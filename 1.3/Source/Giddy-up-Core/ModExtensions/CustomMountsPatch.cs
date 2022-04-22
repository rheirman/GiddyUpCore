using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore
{
    class CustomMountsPatch : DefModExtension
    {
        public List<String> possibleMountsCSV;
        public int mountChance = 0;
        public Dictionary<String, int> possibleMounts = new Dictionary<String, int>();
        
        public void Init()
        {
            foreach(string mountCSV in possibleMountsCSV)
            {
                List<String> list = mountCSV.Split(',').ToList();
                if(list.Count == 2)
                {
                    possibleMounts.Add(list[0], int.Parse(list[1]));
                }
            }
        }

    }


}
