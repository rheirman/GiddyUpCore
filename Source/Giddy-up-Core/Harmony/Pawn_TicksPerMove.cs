using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn), "TicksPerMove")]
    static class Pawn_TicksPerMove
    {
        [HarmonyPriority(Priority.Low)]
        static void Postfix(Pawn __instance, ref bool diagonal, ref int __result)
        {
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance);
            if(pawnData.mount != null)
            {
                if (diagonal)
                {
                    __result = pawnData.mount.TicksPerMoveDiagonal;
                }
                else
                {
                    __result = pawnData.mount.TicksPerMoveCardinal;
                }
            }
        }
    }
}

