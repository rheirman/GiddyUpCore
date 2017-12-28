using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Jobs
{
    public class JobDriver_Mounted : JobDriver
    {
        public Pawn Rider { get { return job.targetA.Thing as Pawn; } }
        ExtendedPawnData riderData;
        bool shouldEnd = false;
        bool isFinished = false;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return waitForRider();
            yield return delegateMovement();
        }
        public override bool TryMakePreToilReservations()
        {
            return true;
        }

        private bool cancelJobIfNeeded(ExtendedPawnData riderData)
        {
            bool result = false;
            if (shouldEnd)
            {
                //Log.Message("cancel job, shouldEnd called");
                result = true;
            }
            Thing thing = pawn as Thing;
            if (Rider.Downed || Rider.Dead || pawn.Downed || pawn.Dead || pawn.IsBurning() || Rider.IsBurning())
            {
                //Log.Message("cancel job, rider downed or dead");
                result = true;
            }
            if (pawn.InMentalState || (Rider.InMentalState && Rider.MentalState.def != MentalStateDefOf.PanicFlee))
            {
                //Log.Message("cancel job, rider or mount in mental state");
                result = true;
            }
            if (!Rider.Spawned)
            {
                if (!Rider.IsColonist)
                {
                    //Log.Message("rider not spawned, despawn");
                    pawn.DeSpawn();
                    result = true;
                }
                else
                {
                    result = true;
                }
            }


            if (!Rider.Drafted && Rider.IsColonist && Rider.mindState.duty.def != DutyDefOf.TravelOrWait && Rider.mindState.duty.def != DutyDefOf.TravelOrLeave)
            {
                //Log.Message("cancel job, rider not drafted while being colonist");
                result = true;
            }

            if (riderData.mount == null)
            {
                //Log.Message("cancel job, rider has no mount");
                result = true;
            }

            if(result == true)
            {
                ReadyForNextToil();
            }

            return result;

        }

        private Toil waitForRider()
        {
            Toil toil = new Toil();

            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.tickAction = delegate
            {
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                if (riderData.mount != null && riderData.mount == pawn)
                {
                    ReadyForNextToil();
                }
                if (Rider.CurJob.def != GUC_JobDefOf.Mount && Rider.CurJob.def != JobDefOf.Vomit && Rider.CurJob.def != JobDefOf.WaitMaintainPosture && riderData.mount == null)
                {
                    Log.Message("cancel wait for rider, rider is not mounting, curJob: " + Rider.CurJob.def.defName);
                    
                    shouldEnd = true;
                    ReadyForNextToil();
                }

            };
            return toil;
        }



        private Toil delegateMovement()
        {
            Toil toil = new Toil();
            toil.defaultCompleteMode = ToilCompleteMode.Never;

            toil.tickAction = delegate
            {
                if (isFinished)
                {
                    return;
                }
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                bool shouldCancel = cancelJobIfNeeded(riderData);
                if (shouldCancel)
                {
                    return;
                }
                pawn.Drawer.tweener = Rider.Drawer.tweener;

                pawn.Position = Rider.Position;
                pawn.Rotation = Rider.Rotation;
                Pawn target = Rider.TargetCurrentlyAimingAt.Thing as Pawn;
                if(target != null && target.Faction.HostileTo(Rider.Faction))
                {
                    pawn.meleeVerbs.TryMeleeAttack(Rider.TargetCurrentlyAimingAt.Thing, this.job.verbToUse, false);
                }

            };

            toil.AddFinishAction(delegate {
                if (!Rider.IsColonist)
                {
                    if (pawn.Faction != null)
                    {
                        pawn.SetFaction(null);
                    }
                }
                isFinished = true;
                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                riderData.reset();
                pawn.Drawer.tweener = new PawnTweener(pawn);
                //pawn.Position = Rider.Position;
            });

            return toil;

        }
    }
}
