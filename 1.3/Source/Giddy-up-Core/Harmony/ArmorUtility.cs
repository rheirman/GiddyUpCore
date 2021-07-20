using GiddyUpCore.Utilities;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(ArmorUtility), "ApplyArmor")]
    class ArmorUtility_ApplyArmor
    {
        static void Postfix(ref float armorRating, Pawn pawn)
        {

            if (IsMountableUtility.IsCurrentlyMounted(pawn) && pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt)
            {
                armorRating *= modExt.armorModifier;
            }
        }
        static void Postfix(ref float damAmount, Pawn pawn, ref bool metalArmor)
        {

            if (IsMountableUtility.IsCurrentlyMounted(pawn) && pawn.def.GetModExtension<CustomStatsPatch>() is CustomStatsPatch modExt)
            {
                damAmount /= modExt.armorModifier;
                metalArmor = modExt.useMetalArmor;
            }
        }
    }
}
