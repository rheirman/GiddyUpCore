using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    static class Pawn_JobTracker_StartJob
    {    
       static bool Prefix(Pawn_JobTracker __instance)
       {
           if (__instance.curDriver != null && __instance.curDriver.pawn != null && __instance.curDriver.pawn.CurJob != null && __instance.curDriver.pawn.CurJob.def == GUC_JobDefOf.Mounted)
           {
                return false;
           }
           return true;
       }
    }

    [HarmonyPatch(typeof(Pawn_JobTracker), "DetermineNextJob")]
    static class Pawn_JobTracker_DetermineNextJob
    {
        static void Postfix(Pawn_JobTracker __instance, ref ThinkResult __result)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
           //Remove mount in case the mount somehow isn't mounted by pawn. 
            if (pawn.IsColonist)
            {
                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                if (pawnData.mount != null)
                {
                    if (pawnData.mount.CurJob == null || pawnData.mount.CurJobDef != GUC_JobDefOf.Mounted)
                    {
                        pawnData.reset();
                    }
                    else if (pawnData.mount.jobs.curDriver is JobDriver_Mounted driver && driver.Rider != pawn)
                    {
                        pawnData.reset();
                    }
                }
            }
            //If a hostile pawn owns an animal, make sure it mounts it whenever possible

            if (pawn.RaceProps.Humanlike && pawn.Faction != null && pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Downed && !pawn.IsBurning() && !pawn.IsPrisoner)
            {

                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                if (pawnData.owning == null || pawnData.owning.Faction != pawn.Faction || pawnData.mount != null || pawnData.owning.Downed || pawnData.owning.Dead || !pawnData.owning.Spawned || pawnData.owning.IsBurning())
                {
                    return;
                }
                QueuedJob qJob = pawn.jobs.jobQueue.FirstOrFallback(null);
                if (qJob != null && (qJob.job.def == GUC_JobDefOf.Mount))
                {
                    return;
                }
                if (__result.Job.def == GUC_JobDefOf.Mount)
                {
                    return;
                }

                Job mountJob = new Job(GUC_JobDefOf.Mount, pawnData.owning);
                mountJob.count = 1;
                __instance.jobQueue.EnqueueFirst(mountJob);


            }
        }
    }
    [HarmonyPatch(typeof(Pawn_JobTracker), "Notify_MasterDraftedOrUndrafted")]
    static class Pawn_JobTracker_Notify_MasterDraftedOrUndrafted
    {
        static bool Prefix(Pawn_JobTracker __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == GUC_JobDefOf.Mounted)
            {
                return false;
            }
            return true;

        }
        
    }
}
