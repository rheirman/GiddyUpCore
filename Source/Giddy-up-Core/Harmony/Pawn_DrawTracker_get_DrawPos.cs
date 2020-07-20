using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{

    [HarmonyPatch(typeof(Pawn_DrawTracker), "get_DrawPos")]
    static class Pawn_DrawTracker_get_DrawPos
    {
        static void Postfix(Pawn_DrawTracker __instance, ref Vector3 __result, ref Pawn ___pawn)
        {
            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();

            if (store == null) { return; }

            ExtendedPawnData pawnData = store.GetExtendedDataFor(___pawn);

            if (pawnData != null && pawnData.mount != null)
            {
                __result += pawnData.drawOffsetCache;
            }
            else if (pawnData != null)
            {
                pawnData.drawOffsetCache = Vector3.zero;
            }
        }
    }
}
