using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn_RotationTracker), "UpdateRotation")]
    class Pawn_RotationTracker_UpdateRotation
    {
        static bool Prefix(ref Pawn_RotationTracker __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if(!pawn.Destroyed && pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
            {
                if (jobDriver.Rider.pather.Moving)
                {
                    if (jobDriver.Rider.pather.curPath == null || jobDriver.Rider.pather.curPath.NodesLeftCount < 1)
                    {
                        return true;
                    }
                    Traverse.Create(__instance).Method("FaceAdjacentCell", new object[] { jobDriver.Rider.pather.nextCell });
                    return false;
                }
                //Log.Message("changing rotation for mech");
                //__instance.Face(jobDriver.Rider.Rotation.FacingCell.ToVector3());
                //return false; 
            }
            return true;
        }
    }
}
