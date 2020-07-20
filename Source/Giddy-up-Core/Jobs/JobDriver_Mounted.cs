using GiddyUpCore.Harmony;
using GiddyUpCore.Storage;
using RimWorld;
using RimWorld.Planet;
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
        public bool interrupted = false;
        bool isFinished = false;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return waitForRider();          
            yield return delegateMovement();
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        private bool shouldCancelJob(ExtendedPawnData riderData)
        {
            if (interrupted)
            {
                //Log.Message("cancel job, shouldEnd called");
                return true;
            }

            if (riderData == null || riderData.mount == null)
            {
                //Log.Message("riderData is null or riderData.mount is null");
                return true;
            }

            Thing thing = pawn as Thing;
            if (Rider.Downed || Rider.Dead || pawn.Downed || pawn.Dead || pawn.IsBurning() || Rider.IsBurning() || Rider.GetPosture() != PawnPosture.Standing)
            {
                //Log.Message("cancel job, rider downed or dead");
                return true;
            }
            if (pawn.InMentalState || (Rider.InMentalState && Rider.MentalState.def != MentalStateDefOf.PanicFlee))
            {
                //Log.Message("cancel job, rider or mount in mental state");
                return true;
            }
            if (!Rider.Spawned)
            {
                if (!Rider.IsColonist && !Rider.Dead)
                {
                    //Log.Message("rider not spawned, despawn");
                    pawn.ExitMap(false, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
                    return true;
                }
                else if (Rider.IsColonist && Rider.GetCaravan() != null)
                {
                    //Log.Message("rider moved to map, despawn");
                    pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
                    return true;
                }
                else
                {
                    //Log.Message("rider died, dismount");
                    return true;
                }
            }

            if (!Rider.Drafted && Rider.IsColonist) //TODO refactor this as a postfix in Giddy-up Caravan. 
            {
                if ((Rider.mindState != null && Rider.mindState.duty != null && (Rider.mindState.duty.def == DutyDefOf.TravelOrWait || Rider.mindState.duty.def == DutyDefOf.TravelOrLeave || Rider.mindState.duty.def == DutyDefOf.PrepareCaravan_GatherPawns || Rider.mindState.duty.def == DutyDefOf.PrepareCaravan_GatherPawns)))
                {
                    if (riderData.caravanMount == pawn)
                    {
                        return false;
                    }
                    return true;
                    //if forming caravan, stay mounted. 
                }
                else if (riderData.owning == pawn)
                {
                    //Log.Message("cancel job, rider not drafted while being colonist");
                    //Log.Message("riderData.owning: " + riderData.owning);
                    return false;
                }
                else
                {
                    return true;
                }
            }



            if (riderData.mount == null)
            {
                //Log.Message("cancel job, rider has no mount");
                return true;
            }
            return false;

        }

        private Toil waitForRider()
        {
            Toil toil = new Toil();

            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.tickAction = delegate
            {
                //Log.Message("waiting for rider");

                if (Rider == null || Rider.Dead || !Rider.Spawned || Rider.Downed || Rider.InMentalState)
                {
                    interrupted = true;
                    ReadyForNextToil();
                    return;
                }

                riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
                if (riderData.mount != null && riderData.mount == pawn)
                {
                    ReadyForNextToil();
                }

                if (Rider.CurJob.def != GUC_JobDefOf.Mount && Rider.CurJob.def != JobDefOf.Vomit && Rider.CurJob.def != JobDefOf.Wait_MaintainPosture && Rider.CurJob.def != JobDefOf.SocialRelax && Rider.CurJob.def != JobDefOf.Wait && riderData.mount == null)
                {
                    //Log.Message("cancel wait for rider, rider is not mounting, curJob: " + Rider.CurJob.def.defName);                  
                    interrupted = true;
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
                if (shouldCancelJob(riderData))
                {
                    ReadyForNextToil();
                    return;
                }
                pawn.Drawer.tweener = Rider.Drawer.tweener;

                pawn.Position = Rider.Position;
                tryAttackEnemy();
                pawn.Rotation = Rider.Rotation;
            };

            toil.AddFinishAction(delegate
            {

                FinishAction();
            });

            return toil;

        }

        private void FinishAction()
        {
            isFinished = true;
            riderData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Rider);
            riderData.reset();
            pawn.Drawer.tweener = new PawnTweener(pawn);
            if (!interrupted)
            {
                pawn.Position = Rider.Position;
            }

            pawn.pather.ResetToCurrentPosition();
        }

        private void tryAttackEnemy()
        {
            Thing targetThing = null;

            if (Rider == null)
                return;

            if (Rider.TargetCurrentlyAimingAt != null)
            {
                targetThing = Rider.TargetCurrentlyAimingAt.Thing;
            }
            else if (Rider.CurJob?.def == JobDefOf.AttackMelee && Rider.CurJob.targetA.Thing.HostileTo(Rider))
            {
                targetThing = Rider.CurJob.targetA.Thing;
            }
            if (targetThing != null && targetThing.HostileTo(Rider))
            {
                if (pawn.meleeVerbs == null || pawn.meleeVerbs.TryGetMeleeVerb(targetThing) == null || !pawn.meleeVerbs.TryGetMeleeVerb(targetThing).CanHitTarget(targetThing))
                {
                    pawn.TryStartAttack(targetThing); //Try start ranged attack if possible
                }
                else
                {
                    pawn.meleeVerbs.TryMeleeAttack(targetThing);
                }
            }
        }
    }
}
