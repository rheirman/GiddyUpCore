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
    //[HarmonyPatch(new Type[] { typeof(Vector3), typeof(bool) })]
    static class Pawn_DrawTracker_get_DrawPos
    {

        static void Postfix(Pawn_DrawTracker __instance, ref Vector3 __result, ref Pawn ___pawn)
        {
            Vector3 drawLoc = __result;
            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();

            if (store == null)
            {
                return;
            }
            ExtendedPawnData pawnData = store.GetExtendedDataFor(___pawn);

            if (pawnData != null && pawnData.mount != null)
            {
                drawLoc = pawnData.mount.Drawer.DrawPos;

                if (pawnData.drawOffset != -1)
                {
                    drawLoc.z = pawnData.mount.Drawer.DrawPos.z + pawnData.drawOffset;
                }
                if (pawnData.mount.def.HasModExtension<DrawingOffsetPatch>())
                {
                    drawLoc += AddCustomOffsets(___pawn, pawnData);
                }
                if (___pawn.Rotation == Rot4.South )
                {
                    AnimalRecord value;
                    bool found = Base.drawSelecter.Value.InnerList.TryGetValue(pawnData.mount.def.defName, out value);
                    Log.Message("pawnData.mount.Drawer.DrawPos.y" + pawnData.mount.Drawer.DrawPos.y);
                    if (found && value.isSelected)
                    {
                        drawLoc.y = pawnData.mount.Drawer.DrawPos.y - 1;
                    }
                }
                __result = drawLoc;
                __result.y += 0.1f;
                Log.Message("__result.y: " + __result.y);
            }
            //if (IsMountableUtility.IsCurrentlyMounted(pawn))
            //{
            //  __result.y -= 5;
            //}
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
