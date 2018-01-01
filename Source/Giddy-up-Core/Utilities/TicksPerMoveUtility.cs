using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Utilities
{
    public class TicksPerMoveUtility
    {
        public static int adjustedTicksPerMove(Pawn pawn, Pawn mount, bool diagonal)
        {
            float adjustedLevel = pawn.skills.GetSkill(SkillDefOf.Animals).levelInt - Mathf.RoundToInt(mount.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
            float animalHandlingOffset = 1f - (adjustedLevel * Base.handlingMovementImpact) / 100f;
            if (diagonal)
            {
                return Mathf.RoundToInt((float)mount.TicksPerMoveDiagonal * animalHandlingOffset);
            }
            else
            {
                return Mathf.RoundToInt((float)mount.TicksPerMoveCardinal * animalHandlingOffset);
            }
        }
    }
}
