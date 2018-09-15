using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Building_Door), "get_BlockedOpenMomentary")]
    static class Building_Door_get_BlockedOpenMomentary
    {
        static void Postfix(Building_Door __instance, ref bool __result)
        {
            if (__result == true) //if true, check for false positives
            {
                List<Thing> thingList = __instance.Position.GetThingList(__instance.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Thing thing = thingList[i];
                    if (thing.def.category == ThingCategory.Item)
                    {
                        __result = true; 
                        return;
                    }
                    else if(thing.def.category == ThingCategory.Pawn) //ignore mounted animals when determining whether door is blocked
                    {
                        Pawn pawn = thing as Pawn;
                        if (pawn.CurJob != null && pawn.CurJob.def == GUC_JobDefOf.Mounted)
                        {
                            __result = false; //dont return, blocking things can still be found
                        }
                        else
                        {
                            __result = true;
                            return;
                        }
                    }
                }
            }

        }
    }

}
