using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn_PlayerSettings), "get_RespectsMaster")]
    static class Pawn_PlayerSettings_get_RespectsMaster
    {
        static void Postfix(Pawn_PlayerSettings __instance, bool __result)
        {
            if(__instance.master != null && Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance.master).owning != null)
            {
                __result = true;
            }
        }
    }
    [HarmonyPatch(typeof(Pawn_PlayerSettings), "get_RespectsAllowedArea")]
    static class Pawn_PlayerSettings_get_RespectsAllowedArea
    {
        static void Postfix(Pawn_PlayerSettings __instance, bool __result)
        {
            if (__instance.master != null && Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance.master).owning != null)
            {
                __result = true;
            }
        }
    }
}
