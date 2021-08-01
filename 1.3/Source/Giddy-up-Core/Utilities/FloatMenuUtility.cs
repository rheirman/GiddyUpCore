using GiddyUpCore.Jobs;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Multiplayer.API;

namespace GiddyUpCore.Utilities
{
    public static class GUC_FloatMenuUtility
    {
        public static void AddMountingOptions(Pawn target, Pawn pawn, List<FloatMenuOption> opts)
        {
            var pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

            if (target.Faction != null && target.Faction != Faction.OfPlayer)
            {
                return;
            }

            if (pawnData.mount == null)
            {
                bool canMount = false;
                if (Base.GiddyUpMechanoidsLoaded && target.RaceProps.IsMechanoid)
                {
                    canMount = true; //additional checking takes place in Giddy-up! Battle Mechs. 
                }
                if (target.RaceProps.Animal)
                {
                    canMount = IsMountableUtility.isMountable(target, out IsMountableUtility.Reason reason);

                    if (!canMount && reason == IsMountableUtility.Reason.NotInModOptions)
                    {
                        opts.Add(new FloatMenuOption("GUC_NotInModOptions".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (target.CurJob != null && (target.InMentalState ||
                        target.IsBurning() ||
                        target.CurJob.def == JobDefOf.LayEgg ||
                        target.CurJob.def == JobDefOf.Nuzzle ||
                        target.CurJob.def == JobDefOf.Lovin ||
                        target.CurJob.def == JobDefOf.Wait_Downed ||
                        target.CurJob.def == GUC_JobDefOf.Mounted
                        ))
                    {
                        opts.Add(new FloatMenuOption("GUC_AnimalBusy".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (!canMount && reason == IsMountableUtility.Reason.NotFullyGrown)
                    {
                        opts.Add(new FloatMenuOption("GUC_NotFullyGrown".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (!canMount && reason == IsMountableUtility.Reason.NeedsObedience)
                    {
                        opts.Add(new FloatMenuOption("GUC_NeedsObedience".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (!canMount && reason == IsMountableUtility.Reason.IsRoped)
                    {
                        opts.Add(new FloatMenuOption("GUC_IsRoped".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }

                }

                if (canMount)
                {
                    Action action = delegate
                    {
                        if (Base.GiddyUpMechanoidsLoaded && target.RaceProps.IsMechanoid)
                        {
                            if (!pawn.Drafted)
                            {
                                //pawn.drafter.Drafted = true; moving to external method to sync across multiplayer clients
                                UpdatePawnDrafted(pawn, true);
                            }
                            if(target.drafter != null && target.Drafted)
                            {
                                //target.drafter.Drafted = false; moving to external method to sync across multiplayer clients
                                UpdatePawnDrafted(target, false);
                            }
                        }
                        Job jobRider = new Job(GUC_JobDefOf.Mount, target);
                        jobRider.count = 1;
                        pawn.jobs.TryTakeOrderedJob(jobRider);
                    };
                    opts.Add(new FloatMenuOption("GUC_Mount".Translate() + " " + target.Name, action, MenuOptionPriority.Low));
                }
            }
            else if (target == pawnData.mount)
            {
                //if (opts.Count > 0) opts.RemoveAt(0);//Remove option to attack your own mount

                Action action = delegate
                {
                    //pawnData.reset(); moving to external method to sync across multiplayer clients
                    ResetPawnData(pawnData);
                };
                opts.Add(new FloatMenuOption("GUC_Dismount".Translate(), action, MenuOptionPriority.High));

            }
        }

        [SyncMethod]
        private static void UpdatePawnDrafted(Pawn pawn, bool draftedStatus)
        {
            pawn.drafter.Drafted = draftedStatus;
        }

        [SyncMethod]
        private static void ResetPawnData(Storage.ExtendedPawnData pawnData)
        {
            pawnData.reset();
        }
    }
}
