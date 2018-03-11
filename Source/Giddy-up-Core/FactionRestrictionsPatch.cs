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
        string allowedDomesticAnimalsCSV = ""; 
        string allowedWildAnimalsCSV = "";
        public int mountChance = -1;
        public int wildAnimalWeight = -1;
        public int domesticAnimalWeight = -1;
        public List<string> getAllowedFarmAnimalsAsList()
        {
            return allowedDomesticAnimalsCSV.Split(',').ToList();
        }
        public List<string> getAllowedWildAnimalsAsList()
        {
            Log.Message("requesting allowedWildAnimalsCSV" + allowedWildAnimalsCSV);
            return allowedWildAnimalsCSV.Split(',').ToList();
        }
    }
}
