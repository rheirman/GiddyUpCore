using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(PawnUIOverlay), "DrawPawnGUIOverlay")]
    class PawnUIOverlay_DrawPawnGUIOverlay
    {
        static bool Prefix(PawnUIOverlay __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue() as Pawn;
            if (pawn.GetExtendedPawnData() is ExtendedPawnData data && data.mount != null)
            {
                Vector2 pos = GenMapUI.LabelDrawPosFor(pawn, -(data.drawOffset + 0.6f));
                GenMapUI.DrawPawnLabel(pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
                return false;
            }
            return true;
        }
    }
}
