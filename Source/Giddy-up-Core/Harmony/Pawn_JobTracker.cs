using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
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
                //Log.Message("target is not valid");
                return;
            }

            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();
            if (store == null)
            {
                //Log.Message("store is null");
                return;
            }

            //Log.Message("wrong duty");
            ExtendedPawnData PawnData = store.GetExtendedDataFor(pawn);
            Lord lord = pawn.GetLord();
            if (lord == null)
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
            else if(lord.CurLordToil.GetType().Name == "LordToil_DefendTraderCaravan") //internal class, therefore this way of accessing. 
            {
                if (PawnData.mount != null)
                {
                    parkAnimal(__instance, pawn, PawnData);
                }
            }




            //Log.Message("determine next job");




            //PawnData.mount.playerSettings.

            //float pawnTargetDistance = DistanceUtility.QuickDistance(pawn.Position, target.Cell);

        }

        private static void mountAnimal(Pawn_JobTracker __instance, Pawn pawn, ExtendedPawnData PawnData, ref ThinkResult __result)
        {
            //Log.Message("mount animal job issued");

            //Job oldJob = __result.Job;
            Job mountJob = new Job(GUC_JobDefOf.Mount, PawnData.owning);
            //mountJob.count = 1;
            __result = new ThinkResult(mountJob, __result.SourceNode, __result.Tag, false);
            __instance.jobQueue.EnqueueFirst(mountJob);
            pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
        }

        private static void parkAnimal(Pawn_JobTracker __instance, Pawn pawn, ExtendedPawnData PawnData)
        {
            //Log.Message("park animal job issued");

            Job dismountJob = new Job(GUC_JobDefOf.Dismount);
            dismountJob.count = 1;
            __instance.jobQueue.EnqueueFirst(dismountJob);
            List<Area> areas = pawn.Map.areaManager.AllAreas;
            foreach(Area area in areas)
            {
                //area.
                IntVec3 loc = area.ActiveCells.First();
            }

            
        }
    }
}
