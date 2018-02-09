using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(LordToil_DefendPoint), "UpdateAllDuties")]
    class LordToil_DefendPoint_UpdateAllDuties
    {
        static bool Prefix(LordToil_DefendPoint __instance)
        {
            LordToilData_DefendPoint data = Traverse.Create(__instance).Field("data").GetValue<LordToilData_DefendPoint>();
            for (int i = 0; i < __instance.lord.ownedPawns.Count; i++)
            {
                Pawn pawn = __instance.lord.ownedPawns[i];
                pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, -1f);
                pawn.mindState.duty.focusSecond = data.defendPoint;
                if (pawn.RaceProps.Animal)
                {
                    Log.Message("setting radius to 1 for animal");
                    pawn.mindState.duty.radius = 1f;
                }
            }
            return false;
        }
    }
}
