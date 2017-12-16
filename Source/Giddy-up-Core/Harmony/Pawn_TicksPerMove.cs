using GiddyUpCore.Storage;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn), "TicksPerMove")]
    static class Pawn_TicksPerMove
    {
        [HarmonyPriority(Priority.Low)]
        static void Postfix(Pawn __instance, ref bool diagonal, ref int __result)
        {
            if(Base.Instance.GetExtendedDataStorage() == null)
            {
                return;
            }
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance);
            if(pawnData.mount != null)
            {
                Pawn mount = pawnData.mount;
                float adjustedLevel = __instance.skills.GetSkill(SkillDefOf.Animals).levelInt - Mathf.RoundToInt(mount.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
                float animalHandlingOffset = 1f - (adjustedLevel * Base.handlingMovementImpact)/100f;
                if (diagonal)
                {
                    __result = Mathf.RoundToInt((float) pawnData.mount.TicksPerMoveDiagonal * animalHandlingOffset);
                }
                else
                {
                    __result = Mathf.RoundToInt((float)pawnData.mount.TicksPerMoveCardinal * animalHandlingOffset);
                }
            }
        }
    }
}

