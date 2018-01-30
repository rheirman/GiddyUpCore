using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Zones
{
    //We only want to address the database once, hence this static class.
    [StaticConstructorOnStartup]
    internal static class StaticDefLoader
    {
        //public static DesignationDef DesStableZone = DefDatabase<DesignationDef>.GetNamed("GUC_Designator_Stable");
    }
}
