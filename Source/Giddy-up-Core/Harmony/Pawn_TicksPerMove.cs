using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn), "TicksPerMove")]
    static class Pawn_TicksPerMove
    {
        [HarmonyPriority(Priority.Low)]
        static void Postfix(Pawn __instance, ref bool diagonal, ref int __result)
        {

            if(Base.Instance.GetExtendedDataStorage() == null)
            {
                return;
            }
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance);
            if (pawnData.mount != null)
            {
                __result = TicksPerMoveUtility.adjustedTicksPerMove(__instance, pawnData.mount, diagonal);
            }


        }
    }
}

