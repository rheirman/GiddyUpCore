using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using HarmonyLib;
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
        static bool Prefix(ref Pawn_RotationTracker __instance, ref Pawn ___pawn)
        {
            if(!___pawn.Destroyed && ___pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
            {
                ___pawn.Rotation = jobDriver.Rider.Rotation;
                return false;      
                //Log.Message("changing rotation for mech");
                //__instance.Face(jobDriver.Rider.Rotation.FacingCell.ToVector3());
                //return false; 
            }
            return true;
        }
    }
}
