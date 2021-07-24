using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    /*  PATCHED FUNCTION PREVIEW

    
    set
	{
		if (value == rotationInt)
		{
			return;
		}
		if (Spawned && (def.size.x != 1 || def.size.z != 1))
		{
			if (def.AffectsRegions)
			{
				Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.");
			}
			RegionListersUpdater.DeregisterInRegions(this, Map);
			Map.thingGrid.Deregister(this);
		}
		rotationInt = value;

    -> PATCH IS INJECTED HERE <-

		if (Spawned && (def.size.x != 1 || def.size.z != 1))
		{
			Map.thingGrid.Register(this);
			RegionListersUpdater.RegisterInRegions(this, Map);
			if (def.AffectsReachability)
			{
				Map.reachability.ClearCache();
			}
		}
	}

     */

    [HarmonyPatch(typeof(Thing), "Rotation", MethodType.Setter)]
    public static class Pawn_RotationTracker_UpdateRotation
    {
        public static MethodInfo mChanged = AccessTools.Method(typeof(Pawn_RotationTracker_UpdateRotation), nameof(Pawn_RotationTracker_UpdateRotation.RotChanged));

        public static void RotChanged(Thing __instance)
        {
            if (!(__instance is Pawn)) { return; }

            var pawn = __instance as Pawn;
            if (!__instance.Destroyed && pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
            {
                __instance.Rotation = jobDriver.Rider.Rotation;
            }
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();

            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (codes[i].opcode == OpCodes.Stfld)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, mChanged);
                }
            }
        }
    }
}
