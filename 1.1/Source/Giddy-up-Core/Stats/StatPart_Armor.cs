using GiddyUpCore.Jobs;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Stats
{
    class StatPart_Armor : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            StringBuilder sb = new StringBuilder();
            if (req.Thing is Pawn pawn && pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted)
            {
                if (pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt && modExt.armorModifier != 1.0f)
                {
                    sb.AppendLine("GUC_GiddyUp".Translate());
                    sb.AppendLine("    " + "GUC_StatPart_MountTypeMultiplier".Translate() + ": " + (modExt.armorModifier).ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
                }  
            }
            return sb.ToString();
        }
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is Pawn pawn && pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted)
            {
                if (pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt && modExt.armorModifier != 1.0f)
                {
                    val *= modExt.armorModifier;
                }
            }
        }
    }
}
