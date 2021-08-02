using GiddyUpCore.Storage;
using GiddyUpCore.Utilities;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(WorkGiver_TakeToPen), "JobOnThing")]
    class WorkGiver_TakeToPen_JobOnThing
    {
        static bool Prefix(WorkGiver_Train __instance, Pawn pawn, Thing t, ref Job __result)
        {
            if (t is Pawn animal && animal.RaceProps.Animal)
            {
                ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();
                bool isCaravanMount = store != null && store.GetExtendedDataFor(animal).caravanRider != null;
                if (IsMountableUtility.IsCurrentlyMounted(animal) || isCaravanMount)
                {
                    __result = null;
                    return false;
                }
            }


            
            return true;
        }
    }
}
