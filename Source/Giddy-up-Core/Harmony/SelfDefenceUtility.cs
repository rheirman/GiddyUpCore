using GiddyUpCore.Jobs;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(SelfDefenseUtility), "ShouldFleeFrom")]
    class SelfDefenceUtility_ShouldFleeFrom
    {
        static bool Prefix(Pawn pawn, ref bool __result)
        {
            if (pawn.RaceProps.Animal)
            {
                Log.Message("ShouldFleeFrom called for animal");
            }
            if(pawn.CurJob != null && pawn.CurJob.def == GUC_JobDefOf.Mounted)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
