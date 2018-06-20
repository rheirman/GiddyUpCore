using GiddyUpCore.Jobs;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Utilities
{
    public static class GUC_FloatMenuUtility
    {
        public static void AddMountingOptions(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
        {
            foreach (LocalTargetInfo current in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
            {

                if (!(current.Thing is Pawn))
                {
                    return;
                }
                Pawn mount = (Pawn)current.Thing;

                if (!mount.RaceProps.Animal)
                {
                    if ((mount.RaceProps.IsMechanoid && Base.GiddyUpWhatTheHackLoaded))
                    {
                        //continue
                    }
                    else
                    {
                        return;
                    }
                }

                var pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

                if (mount.Faction != null && mount.Faction != Faction.OfPlayer)
                {
                    return;
                }

                if (pawnData.mount == null)
                {
                    bool canMount = IsMountableUtility.isMountable(mount, out IsMountableUtility.Reason reason);

                    if (!canMount && reason == IsMountableUtility.Reason.NotInModOptions)
                    {
                        opts.Add(new FloatMenuOption("GUC_NotInModOptions".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (mount.CurJob != null && (mount.InMentalState ||
                        mount.IsBurning() ||
                        mount.CurJob.def == JobDefOf.LayEgg ||
                        mount.CurJob.def == JobDefOf.Nuzzle ||
                        mount.CurJob.def == JobDefOf.Lovin ||
                        mount.CurJob.def == JobDefOf.Wait_Downed ||
                        mount.CurJob.def == GUC_JobDefOf.Mounted
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


                    Action action = delegate
                    {

                        Job jobRider = new Job(GUC_JobDefOf.Mount, mount);
                        jobRider.count = 1;
                        pawn.jobs.TryTakeOrderedJob(jobRider);
                    };
                    opts.Add(new FloatMenuOption("GUC_Mount".Translate() + " " + mount.Name, action, MenuOptionPriority.Low));

                }
                else if (mount == pawnData.mount)
                {
                    if (opts.Count > 0) opts.RemoveAt(0);//Remove option to attack your own mount

                    Action action = delegate
                    {
                        pawnData.reset();
                    };
                    opts.Add(new FloatMenuOption("GUC_Dismount".Translate(), action, MenuOptionPriority.Default));

                }
            }
        }
    }
}
