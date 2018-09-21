using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")]
    static class Pawn_HealthTracker_MakeDowned
    {
        static void Postfix(Pawn_HealthTracker __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();

            //If an enemy animal is downed, make it a wild animal so it can be rescued. 
            if (pawn.RaceProps.Animal && pawn.Faction != null && !pawn.Faction.IsPlayer)
            {
                pawn.SetFaction(null);
            }

            //If the owner of an NPC mount is downed, let the animal flee
            if (pawn.RaceProps.Humanlike && pawn.Faction != null && !pawn.Faction.IsPlayer)
            {
                ExtendedDataStorage dataStorage = Base.Instance.GetExtendedDataStorage();
                if(dataStorage != null)
                {
                    ExtendedPawnData pawnData = dataStorage.GetExtendedDataFor(pawn);
                    if (pawnData != null && pawnData.owning != null && !pawnData.owning.Dead && pawnData.owning.Spawned && pawnData.owning.RaceProps.Animal)
                    {
                        pawnData.owning.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee);
                    }
                }
            }
        }

    }
    [HarmonyPatch(typeof(Pawn_HealthTracker), "SetDead")]
    static class Pawn_HealthTracker_SetDead
    {
        static void Postfix(Pawn_HealthTracker __instance)
        {
            Pawn pawn = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();

            //If the owner of an NPC mount is downed, let the animal flee
            if (pawn.RaceProps.Humanlike && pawn.Faction != null && !pawn.Faction.IsPlayer)
            {
                ExtendedDataStorage dataStorage = Base.Instance.GetExtendedDataStorage();
                if (dataStorage != null)
                {
                    ExtendedPawnData pawnData = dataStorage.GetExtendedDataFor(pawn);
                    if (pawnData != null && pawnData.owning != null && !pawnData.owning.Dead && pawnData.owning.Spawned)
                    {
                        pawnData.owning.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee);
                    }
                }
            }
        }
    }

    

}
