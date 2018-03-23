using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{
    /*
    [HarmonyPatch(typeof(PawnUtility), "GetPosture")]
    class PawnUtility_GetPosture
    {
        static bool Prefix(ref Pawn p, ref PawnPosture __result)
        {
            if(p.jobs != null && p.jobs.curDriver == null)
            {
                p.jobs.curDriver = new JobDriver_Wait();
                //p.jobs.curDriver.ended = true;
                p.jobs.curDriver.job = new Job(JobDefOf.Wait, 10);
                __result = PawnPosture.Standing;
                return false;
            }
            return true;
        }
    }
    */
}
