using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Harmony
{

    [HarmonyPatch(typeof(Thing), "Rotation", MethodType.Setter)]
    public static class Pawn_RotationTracker_UpdateRotation
    {
        public static MethodInfo mChanged = AccessTools.Method(typeof(Pawn_RotationTracker_UpdateRotation), nameof(Pawn_RotationTracker_UpdateRotation.RotChanged));

        public static void RotChanged(Thing __instance)
        {
            if (__instance != null && !(__instance is Pawn)) { return; }

            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();

            if (store == null) { return; }

            var pawn = __instance as Pawn;
            if (!__instance.Destroyed && pawn.jobs != null && pawn.jobs.curDriver is JobDriver_Mounted jobDriver)
            {
                var rotation = jobDriver.Rider.Rotation;
                __instance.Rotation = rotation;

                ExtendedPawnData pawnData = store.GetExtendedDataFor(jobDriver.Rider);

                if (pawnData != null && pawnData.mount != null)
                {
                    pawnData.drawOffsetCache = Vector3.zero;
                    __instance.Rotation = rotation;

                    if (pawnData.drawOffset != -1)
                        pawnData.drawOffsetCache.z = pawnData.drawOffset;

                    if (pawnData.mount.def.HasModExtension<DrawingOffsetPatch>())
                    {
                        pawnData.drawOffsetCache += AddCustomOffsets(jobDriver.Rider, pawnData);
                    }

                    if (rotation == Rot4.South)
                    {
                        AnimalRecord value;
                        bool found = Base.drawSelecter.Value.InnerList.TryGetValue(pawnData.mount.def.defName, out value);
                        if (found && value.isSelected)
                        {
                            pawnData.drawOffsetCache.y = -1;
                        }

                    }
                }
            }
        }

        private static Vector3 AddCustomOffsets(Pawn __instance, ExtendedPawnData pawnData)
        {
            DrawingOffsetPatch customOffsets = pawnData.mount.def.GetModExtension<DrawingOffsetPatch>();
            if (__instance.Rotation == Rot4.North)
            {
                return customOffsets.northOffset;
            }
            if (__instance.Rotation == Rot4.South)
            {
                return customOffsets.southOffset;
            }
            if (__instance.Rotation == Rot4.East)
            {
                return customOffsets.eastOffset;
            }
            return customOffsets.westOffset;
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


