using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Harmony;
using RimWorld;
using GiddyUpCore.Storage;

namespace GiddyUpCore.Harmony
{
        [HarmonyPatch(typeof(ShieldBelt), "DrawWornExtras")]
        static class ShieldBelt_DrawWornExtras
        {
            static bool Prefix(ShieldBelt __instance)
            {
            bool ShouldDisplay = Traverse.Create(__instance).Property("ShouldDisplay").GetValue<bool>();
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance.Wearer);
            Pawn mount = pawnData.mount;

            if (mount == null)
            {
                return true;
            }

            if (__instance.ShieldState == ShieldState.Active && ShouldDisplay)
            {
                float energy = Traverse.Create(__instance).Field("energy").GetValue<float>();
                int lastAbsorbDamageTick = Traverse.Create(__instance).Field("lastAbsorbDamageTick").GetValue<int>();
                Vector3 impactAngleVect = Traverse.Create(__instance).Field("impactAngleVect").GetValue<Vector3>();
                Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
                ;

                float num = Mathf.Lerp(1.2f, 1.55f, energy);
                Vector3 vector = __instance.Wearer.Drawer.DrawPos;
                vector.z += pawnData.drawOffset;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
                if (num2 < 8)
                {
                    float num3 = (float)(8 - num2) / 8f * 0.05f;
                    vector += impactAngleVect * num3;
                    num -= num3;
                }
                float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
            }
            return false;
            }
        }

}
