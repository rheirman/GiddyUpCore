using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(VerbProperties), "AdjustedAccuracy")]
    static class VerbProperties_AdjustedAccuracy
    {
        static void Postfix(VerbProperties __instance, ref Thing equipment, ref float __result)
        {

            if (equipment == null || equipment.holdingOwner == null || !(equipment.holdingOwner.Owner is Pawn_EquipmentTracker))
            {
                return;
            }
            if (equipment == null || equipment.holdingOwner == null || equipment.holdingOwner.Owner == null)
            {
                return;
            }
            Pawn_EquipmentTracker eqt = (Pawn_EquipmentTracker)equipment.holdingOwner.Owner;
            Pawn pawn = Traverse.Create(eqt).Field("pawn").GetValue<Pawn>();
            if (pawn == null || pawn.stances == null)
            {
                return;
            }
            Pawn mount = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn).mount;
            if (mount == null)
            {
                return;
            }
            float adjustedLevel = 5;
            if(pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Animals) is SkillRecord record)
            {
                adjustedLevel = record.levelInt - Mathf.RoundToInt(mount.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
            }
            float animalHandlingOffset = adjustedLevel * Base.handlingAccuracyImpact;
            float factor = (100f - ((float)Base.accuracyPenalty.Value - animalHandlingOffset)) / 100f;
            __result *= factor;
        }
    }
}
