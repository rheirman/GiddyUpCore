using GiddyUpCore.Jobs;
using HarmonyLib;
using RimWorld;
using Multiplayer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{

    [HarmonyPatch(typeof(Pawn_PlayerSettings), "GetGizmos")]
    static class Pawn_PlayerSettings_GetGizmos
    {
        //purpose: Make sure animals don't throw of their rider when released. 

        static bool Prefix(Pawn_PlayerSettings __instance, ref IEnumerable<Gizmo> __result)
        {
            __result = helpIterator(__instance);
            return false;
        }
        //Almost literal copy of vanilla code. Only added a check around the EndCurrentJob call
        static IEnumerable<Gizmo> helpIterator(Pawn_PlayerSettings __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue() as Pawn;
            if (pawn.Drafted)
            {
                if (PawnUtility.SpawnedMasteredPawns(pawn).Any((Pawn p) => p.training.HasLearned(TrainableDefOf.Release)))
                {
                    yield return new Command_Toggle
                    {
                        defaultLabel = "CommandReleaseAnimalsLabel".Translate(),
                        defaultDesc = "CommandReleaseAnimalsDesc".Translate(),
                        icon = TexCommand.ReleaseAnimals,
                        hotKey = KeyBindingDefOf.Misc7,
                        isActive = (() => __instance.animalsReleased),
                        toggleAction = () => UpdateAnimalRelease(pawn, __instance)

                    };
                }
            }
        }

        [SyncMethod]
        private static void UpdateAnimalRelease(Pawn pawn, Pawn_PlayerSettings __instance)
        {
            __instance.animalsReleased = !__instance.animalsReleased;
            if (__instance.animalsReleased)
            {
                foreach (Pawn current in PawnUtility.SpawnedMasteredPawns(pawn))
                {
                    if (current.caller != null)
                    {
                        current.caller.Notify_Released();
                    }
                    if (current.CurJob.def != GUC_JobDefOf.Mounted)
                    {
                        current.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
                    }
                }
            }
        }

        //I'd preferably use this transpiler, but because the target method is an iterator method this doesn't work like it normally does. Therefore the prefix  
        /*
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Log.Message("calling transpiler");

            var instructionsList = new List<CodeInstruction>(instructions);
            for (var i = 0; i < instructionsList.Count; i++)
            {

                //Log.Message(instructionsList[i].labels.ToString());
                if(instructionsList[i].operand != null)
                //Log.Message(instructionsList[i].operand.ToString());

                if (instructionsList[i].operand == typeof(Pawn_JobTracker).GetMethod("EndCurrentJob"))
                {
                    //Log.Message("found EndCurrentJob");
                    yield return new CodeInstruction(OpCodes.Call, typeof(Pawn_PlayerSettings_GetGizmos).GetMethod("EndCurrentJob"));//Injected code     
                }
                else
                {
                    yield return instructionsList[i];
                }

            }
        }
        public static void EndCurrentJob(Pawn pawn, JobCondition condition, bool startNewJob = true)
        {
            //Log.Message("EndCurrentJob called, pawn: " + pawn.Name);
        }
        */

    }

}
