using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Utilities
{
    public class NPCMountUtility
    {

        public static bool generateMounts(List<Pawn> list, IncidentParms parms, int inBiomeWeight, int outBiomeWeight, int nonWildWeight, int mountChance, int mountChanceTribal)
        {
            Map map = parms.target as Map;
            if (map == null)
            {
                Caravan caravan = (Caravan)parms.target;
                int tile = caravan.Tile;
                map = Current.Game.FindMap(tile);
                if (map == null)
                {
                    return false;
                }
            }
            Predicate<PawnKindDef> isAnimal = (PawnKindDef d) => d.race != null && d.race.race.Animal;

            int enemyMountChance = getMountChance(parms, mountChance, mountChanceTribal);
            if (enemyMountChance == -1)//wrong faction
            {
                return false;
            }
            Random rand = new Random(DateTime.Now.Millisecond);

            int totalWeight = inBiomeWeight + outBiomeWeight + nonWildWeight;
            float inBiomeWeightNormalized = (float)inBiomeWeight / (float)totalWeight * 100f;
            float outBiomeWeightNormalized = (float)outBiomeWeight / (float)totalWeight * 100f;


            foreach (Pawn pawn in list)
            {
                //TODO add chance
                int rndInt = rand.Next(1, 100);
                if (enemyMountChance <= rndInt || !pawn.RaceProps.Humanlike)
                {
                    continue;
                }
                rndInt = rand.Next(1, 100);
                int pawnHandlingLevel = pawn.skills.GetSkill(SkillDefOf.Animals).Level;
                PawnKindDef pawnKindDef;
                if (rndInt <= inBiomeWeightNormalized)//TODO: change this
                {
                    pawnKindDef = (from a in map.Biome.AllWildAnimals
                                   where map.mapTemperature.SeasonAcceptableFor(a.race) && isMountable(a.defName)
                                   select a).RandomElementByWeight((PawnKindDef def) => calculateCommonality(def, map, pawnHandlingLevel));
                }
                else if (rndInt <= inBiomeWeightNormalized + outBiomeWeightNormalized)
                {
                    pawnKindDef = (from a in DefDatabase<PawnKindDef>.AllDefs
                                   where isAnimal(a) && a.wildSpawn_spawnWild && map.mapTemperature.SeasonAcceptableFor(a.race) && isMountable(a.defName)
                                   select a).RandomElementByWeight((PawnKindDef def) => calculateCommonality(def, map, pawnHandlingLevel));
                }
                else
                {
                    pawnKindDef = (from a in DefDatabase<PawnKindDef>.AllDefs
                                   where isAnimal(a) && !a.wildSpawn_spawnWild && map.mapTemperature.SeasonAcceptableFor(a.race) && isMountable(a.defName)
                                   select a).RandomElementByWeight((PawnKindDef def) => 1 - def.RaceProps.wildness);
                }

                if (pawnKindDef == null)
                {
                    Log.Error("No spawnable animals right now.");
                    return false;
                }


                Pawn animal = PawnGenerator.GeneratePawn(pawnKindDef, parms.faction);
                GenSpawn.Spawn(animal, pawn.Position, map, parms.spawnRotation, false);
                ExtendedPawnData pawnData = GiddyUpCore.Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                pawnData.mount = animal;
                TextureUtility.setDrawOffset(pawnData);

                if (animal.jobs == null)
                {
                    animal.jobs = new Pawn_JobTracker(animal);
                }

                Job jobAnimal = new Job(GUC_JobDefOf.Mounted, pawn);
                jobAnimal.count = 1;
                animal.jobs.TryTakeOrderedJob(jobAnimal);

            }
            return true;
        }


        private static float calculateCommonality(PawnKindDef def, Map map, int pawnHandlingLevel)
        {
            float commonality = map.Biome.CommonalityOfAnimal(def);
            //minimal level to get bonus. 
            pawnHandlingLevel = pawnHandlingLevel > 5 ? pawnHandlingLevel - 5 : 0;

            //Common animals more likely when pawns have low handling, and rare animals more likely when pawns have high handling.  
            float commonalityAdjusted = commonality * ((15f - (float)commonality)) / 15f + (1 - commonality) * ((float)pawnHandlingLevel) / 15f;
            //Wildness decreases the likelyhood of the mount being picked. Handling level mitigates this. 
            float wildnessPenalty = 1 - (def.RaceProps.wildness * ((15f - (float)pawnHandlingLevel) / 15f));

            //Log.Message("name: " + def.defName + ", commonality: " + commonality + ", pawnHandlingLevel: " + pawnHandlingLevel + ", wildness: " + def.RaceProps.wildness + ", commonalityBonus: " + commonalityAdjusted + ", wildnessPenalty: " + wildnessPenalty + ", result: " + commonalityAdjusted * wildnessPenalty);
            return commonalityAdjusted * wildnessPenalty;
        }

        private static int getMountChance(IncidentParms parms, int mountChance, int mountChanceTribal)
        {
            if (parms.faction.def == FactionDefOf.Tribe)
            {
                return mountChanceTribal;
            }
            else if (parms.faction.def != FactionDefOf.Spacer && parms.faction.def != FactionDefOf.SpacerHostile && parms.faction.def != FactionDefOf.Mechanoid)
            {
                return mountChance;
            }
            else
            {
                return -1;
            }
        }

        //TODO: refactor this, should be in core
        private static bool isMountable(String animalName)
        {
            GiddyUpCore.AnimalRecord value;
            bool found = GiddyUpCore.Base.animalSelecter.Value.InnerList.TryGetValue(animalName, out value);
            if (found && value.isSelected)
            {
                return true;
            }
            return false;
        }
    }
}

