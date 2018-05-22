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
                if (!(current.Thing is Pawn) || !((Pawn)current.Thing).RaceProps.Animal)
                {
                    return;
                }

                var pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                Pawn animal = (Pawn)current.Thing;

                if (animal.Faction != null && animal.Faction != Faction.OfPlayer)
                {
                    return;
                }

                if (pawnData.mount == null)
                {
                    bool canMount = IsMountableUtility.isMountable(animal, out IsMountableUtility.Reason reason);

                    if (!canMount && reason == IsMountableUtility.Reason.NotInModOptions)
                    {
                        opts.Add(new FloatMenuOption("GUC_NotInModOptions".Translate(), null, MenuOptionPriority.Low));
                        return;
                    }
                    if (animal.CurJob != null && (animal.InMentalState ||
                        animal.IsBurning() ||
                        animal.CurJob.def == JobDefOf.LayEgg ||
                        animal.CurJob.def == JobDefOf.Nuzzle ||
                        animal.CurJob.def == JobDefOf.Lovin ||
                        animal.CurJob.def == JobDefOf.WaitDowned ||
                        animal.CurJob.def == GUC_JobDefOf.Mounted
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

                        Job jobRider = new Job(GUC_JobDefOf.Mount, animal);
                        jobRider.count = 1;
                        pawn.jobs.TryTakeOrderedJob(jobRider);
                    };
                    opts.Add(new FloatMenuOption("GUC_Mount".Translate() + " " + animal.Name, action, MenuOptionPriority.Low));

                }
                else if (animal == pawnData.mount)
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
