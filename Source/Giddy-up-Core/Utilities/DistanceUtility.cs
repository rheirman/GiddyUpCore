using GiddyUpCore.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Utilities
{
    public static class DistanceUtility
    {
        //TODO fix this
        public static float QuickDistance(IntVec3 a, IntVec3 b)
        {
            float arg_1D_0 = (float)(a.x - b.x);
            float num = (float)(a.z - b.z);
            return arg_1D_0 * arg_1D_0 + num * num;
        }
        public static LocalTargetInfo GetFirstTarget(Job job, TargetIndex index)
        {
            if (!job.GetTargetQueue(index).NullOrEmpty<LocalTargetInfo>())
            {
                return job.GetTargetQueue(index)[0];
            }
            return job.GetTarget(index);
        }
        public static IntVec3 getClosestAreaLoc(Pawn pawn, Area_GU areaFound)
        {
            IntVec3 targetLoc = new IntVec3();
            double minDistance = double.MaxValue;
            foreach (IntVec3 loc in areaFound.ActiveCells)
            {
                double distance = DistanceUtility.QuickDistance(loc, pawn.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetLoc = loc;
                }
            }
            return targetLoc;
        }
    }
}
