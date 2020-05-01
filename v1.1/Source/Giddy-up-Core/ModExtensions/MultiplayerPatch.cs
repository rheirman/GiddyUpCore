using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Multiplayer.API;
using RimWorld;
using Verse;
using Verse.AI;

namespace GiddyUpCore.ModExtensions
{
    [StaticConstructorOnStartup]
    public static class MultiplayerPatch
    {
        internal static readonly HarmonyLib.Harmony giddyUpMultiplayerHarmony = new HarmonyLib.Harmony("giddyup.multiplayer.compat");

        public static Dictionary<object, Pawn> backward;
        public static Dictionary<Pawn, object> forward;

        static MultiplayerPatch()
        {
            if (MP.enabled)
            {
                MP.RegisterAll();

                Type type = AccessTools.TypeByName("GiddyUpCore.Storage.ExtendedDataStorage");

                giddyUpMultiplayerHarmony.Patch(AccessTools.Constructor(type),
                    postfix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(ExtendedDataStoragePostfix)));

                giddyUpMultiplayerHarmony.Patch(AccessTools.Method(type, "GetExtendedDataFor", new Type[] { typeof(Pawn) }),
                    postfix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(GetExtendedDataForPostfix)));

                giddyUpMultiplayerHarmony.Patch(AccessTools.Method(type, "DeleteExtendedDataFor", new Type[] { typeof(Pawn) }),
                    postfix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(DeleteExtendedDataForPostfix)));

                type = AccessTools.TypeByName("GiddyUpCore.Storage.ExtendedPawnData");

                MP.RegisterSyncWorker<object>(ExtendedPawnData, type);
            }
        }

        static void ExtendedDataStoragePostfix()
        {
            forward = new Dictionary<Pawn, object>();
            backward = new Dictionary<object, Pawn>();
        }

        static void GetExtendedDataForPostfix(Pawn pawn, object __result)
        {
            if (forward.ContainsKey(pawn))
            {
                return;
            }
            forward.Add(pawn, __result);
            if (backward.ContainsKey(pawn))
            {
                return;
            }
            backward.Add(__result, pawn);
        }

        static void DeleteExtendedDataForPostfix(Pawn pawn)
        {
            if (!forward.ContainsKey(pawn))
            {
                return;
            }
            backward.Remove(forward[pawn]);
            forward.Remove(pawn);
        }

        static void ExtendedPawnData(SyncWorker sync, ref object obj)
        {
            Pawn pawn = null;

            if (sync.isWriting)
            {
                pawn = backward[obj];

                if (pawn != null)
                {
                    sync.Write(pawn);
                }

            }
            else
            {
                pawn = sync.Read<Pawn>();

                obj = forward[pawn];
            }
        }
    }
}
