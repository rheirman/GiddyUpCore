using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using GiddyUpCore.Storage;
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
            if (!MP.enabled)
            {
                
            }
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
            
            var rngFixMethods = new[]
            {
                "GiddyUpCore.Utilities.NPCMountUtility:generateMounts",
                "GiddyUpCore.Utilities.NPCMountUtility:determinePawnKind",
            };
            PatchPushPopRand(rngFixMethods); // Needed for other faction pawn arrival
            MP.RegisterSyncMethod(typeof(Jobs.JobDriver_Mounted), nameof(Jobs.JobDriver_Mounted.tryAttackEnemy)); // Needed to sync aiming pie
            MP.RegisterSyncMethod(typeof(Zones.Designator_GU), nameof(Zones.Designator_GU.ProcessInput)); // Needed to sync placing of Giddy Zones
            MP.RegisterSyncMethod(typeof(ExtendedPawnData), nameof(Storage.ExtendedPawnData.reset));
            MP.RegisterSyncMethod(typeof(Jobs.JobDriver_Mounted), nameof(Jobs.JobDriver_Mounted.delegateMovement));
            MP.RegisterSyncMethod(typeof(Jobs.JobDriver_Mount), nameof(Jobs.JobDriver_Mount.FinishAction));
            
            MP.RegisterSyncMethod(typeof(Utilities.DrawUtility), nameof(Utilities.DrawUtility.CustomDrawer_Tabs)); // Needed to sync with arbiter START
            MP.RegisterSyncMethod(typeof(Utilities.DrawUtility), nameof(Utilities.DrawUtility.CustomDrawer_Filter));
            MP.RegisterSyncMethod(typeof(Utilities.DrawUtility), nameof(Utilities.DrawUtility.filterAnimals));
            MP.RegisterSyncMethod(typeof(Utilities.DrawUtility), nameof(Utilities.DrawUtility.CustomDrawer_MatchingAnimals_active));
            MP.RegisterSyncMethod(typeof(Utilities.TextureUtility), nameof(Utilities.TextureUtility.setDrawOffset)); // END
            
            MP.RegisterSyncMethod(typeof(Utilities.TextureUtility), nameof(Harmony.Pawn_DrawTracker_get_DrawPos));

        }

        static void ExtendedDataStoragePostfix()
        {
            forward = new Dictionary<Pawn, object>();
            backward = new Dictionary<object, Pawn>();
        }

        static void GetExtendedDataForPostfix(Pawn pawn, object __result)
        {
            if (forward != null && !forward.ContainsKey(pawn)) // null check is required in case mod which adds pawn data is removed mid-save
            {
                forward.Add(pawn, __result);
            }
            if (backward!= null && !backward.ContainsKey(__result))
            {
                backward.Add(__result, pawn);
            }
        }

        static void DeleteExtendedDataForPostfix(Pawn pawn)
        {
            if (forward.ContainsKey(pawn))
            {
                backward.Remove(forward[pawn]);
                forward.Remove(pawn);
            }
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
        static void FixRNGPre() => Rand.PushState();
        static void FixRNGPos() => Rand.PopState();
        internal static void PatchPushPopRand(MethodBase method, HarmonyMethod transpiler = null)
        {
            giddyUpMultiplayerHarmony.Patch(method,
                prefix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(FixRNGPre)),
                postfix: new HarmonyMethod(typeof(MultiplayerPatch), nameof(FixRNGPos)),
                transpiler: transpiler);
        }
        internal static void PatchPushPopRand(string method, HarmonyMethod transpiler = null)
            => PatchPushPopRand(AccessTools.Method(method), transpiler);
        internal static void PatchPushPopRand(string[] methods, HarmonyMethod transpiler = null)
        {
            foreach (var method in methods)
            {
                PatchPushPopRand(AccessTools.Method(method), transpiler);   
            }
        }
    }
}
