using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Zones
{
    static class Extensions
    {
        public static bool TryMakeNewStableArea(this AreaManager instance, AllowedAreaMode mode, out Area_Stable area)
        {
            /*
            if (!instance.CanMakeNewAllowed(mode))
            {
                area = null;
                return false;
            }
            */
            area = new Area_Stable(instance);
            List<Area> areaManager_areas = Traverse.Create(instance).Field("areas").GetValue<List<Area>>();
            areaManager_areas.Add(area);
            return true;
        }
    }
}
