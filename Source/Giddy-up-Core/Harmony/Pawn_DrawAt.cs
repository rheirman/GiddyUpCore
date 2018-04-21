using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn), "DrawAt")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(bool) })]
    static class Pawn_DrawAt
    {

        static bool Prefix(Pawn __instance, Vector3 drawLoc, bool flip = false)
        {
            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();

            if (store == null)
            {
                return true;
            }
            ExtendedPawnData pawnData = store.GetExtendedDataFor(__instance);

            if (pawnData != null && pawnData.mount != null)
            {
                drawLoc = pawnData.mount.Drawer.DrawPos;

                if (pawnData.drawOffset != -1)
                {
                    drawLoc.z = pawnData.mount.Drawer.DrawPos.z + pawnData.drawOffset;
                }
                if (pawnData.mount.def.HasModExtension<DrawingOffsetPatch>())
                {
                    drawLoc += AddCustomOffsets(__instance, pawnData);
                }

                if (__instance.Rotation == Rot4.South )
                {
                    AnimalRecord value;
                    bool found = Base.drawSelecter.Value.InnerList.TryGetValue(pawnData.mount.def.defName, out value);
                    if (found && value.isSelected)
                    {
                        drawLoc.y = pawnData.mount.Drawer.DrawPos.y - 1;
                    }
                }
                __instance.Drawer.DrawAt(drawLoc);
                return false;
            }
            return true;
        }

        private static void DrawGraphicsOverlay()
        {

        }

        private static Vector3 AddCustomOffsets(Pawn __instance, ExtendedPawnData pawnData)
        {
            DrawingOffsetPatch customOffsets = pawnData.mount.def.GetModExtension<DrawingOffsetPatch>();
            if (__instance.Rotation == Rot4.North)
            {
                return customOffsets.northOffset;
            }
            if (__instance.Rotation == Rot4.South)
            {
                return customOffsets.southOffset;
            }
            if (__instance.Rotation == Rot4.East)
            {
                return customOffsets.eastOffset;
            }
            return customOffsets.westOffset;
        }
    }
}
