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
    [HarmonyPatch(typeof(Projectile), "Launch")]
    [HarmonyPriority(Priority.High)]
    [HarmonyPatch(new Type[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(Thing), typeof(ThingDef) })]
    static class Projectile_Launch
    {
        static void Prefix(ref Thing launcher, ref Vector3 origin)
        {
            if (!(launcher is Pawn))
            {
                return;
            }
            Pawn pawn = launcher as Pawn;
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

            if (pawnData.drawOffset > -1)
            {
                origin.z += pawnData.drawOffset;
            }
        }
    }
    */
}
