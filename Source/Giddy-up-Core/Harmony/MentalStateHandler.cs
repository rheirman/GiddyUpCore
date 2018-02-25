using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState")]
    class MentalStateHandler_TryStartMentalState
    {
        static bool Prefix(MentalStateHandler __instance, MentalStateDef stateDef)
        {
            if(stateDef == MentalStateDefOf.PanicFlee)
            {
                Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
                if(pawn.CurJob != null && pawn.CurJob.def == GUC_JobDefOf.Mounted)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
