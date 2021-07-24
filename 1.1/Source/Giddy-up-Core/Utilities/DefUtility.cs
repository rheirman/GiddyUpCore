using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Utilities
{
    public class DefUtility
    {

        public static List<ThingDef> getAnimals()
        {
            //TODO: adapt this!
            Predicate<ThingDef> isAnimal = (ThingDef d) => d.race !=null && d.race.Animal;
            List<ThingDef> animals = new List<ThingDef>();
            foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
                                          where isAnimal(td)
                                          select td)
            {
                animals.Add(thingDef);
            }
            return animals;
        }

    }
}
