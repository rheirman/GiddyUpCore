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
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance);

            if (pawnData.mount != null)
            {
                drawLoc = pawnData.mount.Drawer.DrawPos;

                if (pawnData.drawOffset != -1)
                {
                    drawLoc.z = pawnData.mount.Drawer.DrawPos.z + pawnData.drawOffset;
                }
                if(__instance.Rotation == Rot4.South )
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


    }
}
