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
    /*
    [HarmonyPatch(typeof(GenDraw), "DrawAimPie")]
    static class GenDraw_DrawAimPie
    {

        static bool Prefix(ref Thing shooter, ref LocalTargetInfo target, ref int degreesWide, ref float offsetDist)
        {
            if (!(shooter is Pawn))
            {
                return true;
            }
            Pawn pawn = shooter as Pawn;
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

            if(pawnData.drawOffset > -1){
                float facing = 0f;
                if (target.Cell != shooter.Position)
                {
                    if (target.Thing != null)
                    {
                        facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
                    }
                    else
                    {
                        facing = (target.Cell - shooter.Position).AngleFlat;
                    }
                }
                GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, pawnData.drawOffset), facing, degreesWide);
                return false;
            }
            return true;

        }
    }
    */
}
