using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{

    [HarmonyPatch(typeof(AreaManager), "AddStartingAreas")]
    static class AreaManager_AddStartingAreas
    {
        static void Postfix(AreaManager __instance)
        {
            List<Area> areas = Traverse.Create(__instance).Field("areas").GetValue<List<Area>>();
            //Log.Message("adding extra area");
            areas.Add(new Area_Home(__instance));
        }
    }
}
