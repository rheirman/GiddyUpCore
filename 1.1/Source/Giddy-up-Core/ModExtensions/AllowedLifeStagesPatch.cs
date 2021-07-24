using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore
{
    class AllowedLifeStagesPatch : DefModExtension
    {
        //Can be used in xml patches to allow other life stages than the final one. 
     
        string allowedLifeStagesCSV = "";

        public List<int> getAllowedLifeStagesAsList()
        {
            List <int> result = new List<int>();
            if (!allowedLifeStagesCSV.NullOrEmpty())
            {
                result = allowedLifeStagesCSV.Split(',').ToList().Select(int.Parse).ToList();
            }
            return result;
        }
    }
}
