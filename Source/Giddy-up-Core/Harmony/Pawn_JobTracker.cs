using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using GiddyUpCore.Zones;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace GiddyUpCore.Harmony
{


    [HarmonyPatch(typeof(Pawn_JobTracker), "DetermineNextJob")]
    static class Pawn_JobTracker_DetermineNextJob
    {


        static void Prefix(Pawn_JobTracker __instance, ref ThinkResult __result)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();

            if (pawn.RaceProps.Animal && pawn.Faction != Faction.OfPlayer)
            {

                if(pawn.GetLord() != null && pawn.GetLord().CurLordToil is LordToil_DefendPoint || pawn.GetLord().CurLordToil.GetType().Name == "LordToil_DefendTraderCaravan")
                {
                    Log.Message("set duty radius 4 for animal");
                    //pawn.mindState.duty.radius = 4f;
                    if( __result.SourceNode is JobGiver_Wander)
                    {
                        JobGiver_Wander jgWander = (JobGiver_Wander)__result.SourceNode;
                        Traverse.Create(__result.SourceNode).Field("wanderRadius").SetValue(5f);
                    }

                }
                
            }
        }
        static void Postfix(Pawn_JobTracker __instance, ref ThinkResult __result)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();


            if (pawn.IsColonistPlayerControlled || pawn.RaceProps.Animal)
            {            
                return;
            }

            LocalTargetInfo target = DistanceUtility.GetFirstTarget(__result.Job, TargetIndex.A);
            if (!target.IsValid)
            {
                return;
            }

            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();
            if (store == null)
            {
                return;
            }

            //Log.Message("wrong duty");
            ExtendedPawnData PawnData = store.GetExtendedDataFor(pawn);
            Lord lord = pawn.GetLord();
            if (lord == null)
            {
                return;

            }
            QueuedJob qJob = pawn.jobs.jobQueue.FirstOrFallback(null);
            if(qJob != null && (qJob.job.def == GUC_JobDefOf.Dismount))
            {
                return;
            }

            Log.Message("curLordToil: " + pawn.GetLord().CurLordToil.ToString() + ", pawn name: " + pawn.Name);
            Log.Message("lordJob: " + pawn.GetLord().LordJob + ", pawn name: " + pawn.Name);
            Log.Message("lord.CurLordToil.GetType().Name" + lord.CurLordToil.GetType().Name);

            if (lord.CurLordToil is LordToil_ExitMapAndEscortCarriers)
            {
                if (PawnData.owning != null && PawnData.mount == null && !PawnData.owning.Downed && PawnData.owning.Spawned)
                {
                    mountAnimal(__instance, pawn, PawnData, ref __result);

                }
            }
            else if(lord.CurLordToil.GetType().Name == "LordToil_DefendTraderCaravan" || lord.CurLordToil is LordToil_DefendPoint) //first option is internal class, hence this way of accessing. 
            {
                if (PawnData.mount != null)
                {
                    parkAnimal(__instance, pawn, PawnData);
                }
            }
        }
        
        private static void mountAnimal(Pawn_JobTracker __instance, Pawn pawn, ExtendedPawnData pawnData, ref ThinkResult __result)
        {
            Job mountJob = new Job(GUC_JobDefOf.Mount, pawnData.owning);
            __result = new ThinkResult(mountJob, __result.SourceNode, __result.Tag, false);
            __instance.jobQueue.EnqueueFirst(mountJob);
            pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
        }

        private static void parkAnimal(Pawn_JobTracker __instance, Pawn pawn, ExtendedPawnData pawnData)
        {
            Area_Stable areaFound = (Area_Stable) pawn.Map.areaManager.GetLabeled(Base.STABLE_LABEL);
            IntVec3 targetLoc = pawn.Position;

            if(areaFound != null)
            {
                targetLoc = DistanceUtility.getClosestAreaLoc(pawn, areaFound);
            }

            Job dismountJob = new Job(GUC_JobDefOf.Dismount);
            dismountJob.count = 1;
            __instance.jobQueue.EnqueueFirst(dismountJob);
            __instance.jobQueue.EnqueueFirst(new Job(JobDefOf.Goto, targetLoc));
            PawnDuty animalDuty = pawnData.mount.mindState.duty;
            if(animalDuty != null)
            {
                animalDuty.focus = new LocalTargetInfo(targetLoc);
            }
        }


    }
}
