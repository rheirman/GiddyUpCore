using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore
{
    class FactionRestrictionsPatch : DefModExtension
    {
        //Can be used in xml patches to restrict animals per faction. 
        string allowedNonWildAnimalsCSV = ""; 
        string allowedWildAnimalsCSV = "";
        public int mountChance = -1;
        public int wildAnimalWeight = -1;
        public int nonWildAnimalWeight = -1;
        public List<string> getAllowedNonWildAnimalsAsList()
        {
            return allowedNonWildAnimalsCSV.Split(',').ToList();
        }
        public List<string> getAllowedWildAnimalsAsList()
        {
            Log.Message("requesting allowedWildAnimalsCSV" + allowedWildAnimalsCSV);
            return allowedWildAnimalsCSV.Split(',').ToList();
        }
    }
}
