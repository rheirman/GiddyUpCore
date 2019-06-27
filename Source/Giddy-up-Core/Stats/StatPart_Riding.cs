using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Stats
{
    class StatPart_Riding : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            StringBuilder sb = new StringBuilder();

            if (req.Thing is Pawn pawn)
            {
                if (Base.Instance.GetExtendedDataStorage() == null)
                {
                    return "";
                }
                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);

                if(pawnData.mount != null)
                {
                    float mountSpeed = pawnData.mount.GetStatValue(StatDefOf.MoveSpeed);
                    sb.AppendLine("GUC_GiddyUp".Translate());
                    sb.AppendLine("    " + "GUC_StatPart_MountMoveSpeed".Translate() + ": " + mountSpeed.ToStringByStyle(ToStringStyle.FloatMaxTwo));
                }

                if(pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
                {
                    sb.AppendLine("Giddy up!");
                    float adjustedLevel = 0;
                    if (jobDriver.Rider.skills != null && jobDriver.Rider.skills.GetSkill(SkillDefOf.Animals) is SkillRecord skill)
                    {
                        adjustedLevel = skill.levelInt - Mathf.RoundToInt(pawn.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
                        float animalHandlingOffset = 1f + (adjustedLevel * Base.handlingMovementImpact) / 100f;
                        sb.AppendLine("    " + "GUC_StatPart_HandlingMultiplier".Translate() + ": " + animalHandlingOffset.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
                        sb.AppendLine("        " + "GUC_StatPart_HandlingSkill".Translate() + ": " + skill.levelInt);
                        sb.AppendLine("        " + "GUC_StatPart_SkillReq".Translate() + ": " + Mathf.RoundToInt(pawn.GetStatValue(StatDefOf.MinimumHandlingSkill, true)));
                        sb.AppendLine("        " + "GUC_StatPart_LevelsAbove".Translate() + ": " + adjustedLevel);
                        sb.AppendLine("        " + "GUC_StatPart_HandlingMovementImpact".Translate() + ": " + Base.handlingMovementImpact.Value.ToStringByStyle(ToStringStyle.PercentOne));
                    }
                    if (pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt)
                    {
                        sb.AppendLine("    " + "GUC_StatPart_MountTypeMultiplier".Translate() + ": " + modExt.speedModifier.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
                    }
                }


               
            }
            return sb.ToString();
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if(req.Thing is Pawn pawn)
            {
                if (Base.Instance.GetExtendedDataStorage() == null)
                {
                    return;
                }
                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                if (pawnData.mount != null)
                {
                    float mountSpeed = pawnData.mount.GetStatValue(StatDefOf.MoveSpeed);
                    val = mountSpeed;
                    return;
                }
                if (pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
                {
                    float adjustedLevel = 0;
                    if (jobDriver.Rider.skills != null && jobDriver.Rider.skills.GetSkill(SkillDefOf.Animals) is SkillRecord skill)
                    {
                        adjustedLevel = skill.levelInt - Mathf.RoundToInt(pawn.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
                    }
                    float animalHandlingOffset = 1f + (adjustedLevel * Base.handlingMovementImpact) / 100f;
                    val *= animalHandlingOffset;
                    if (pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt)
                    {
                        float customSpeedModifier = modExt.speedModifier;
                        val *= customSpeedModifier;
                    }
                }


               
            }
            
        }
    }
}
