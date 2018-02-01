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
    }
}
