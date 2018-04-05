using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore
{
    class DrawingOffsetPatch : DefModExtension
    {
        //Can be used in xml patches to apply custom offsets to how riders are drawn on their animals. 
     
        string northOffsetCSV = "";
        string southOffsetCSV = "";
        string eastOffsetCSV = "";
        string westOffsetCSV = "";
        public Vector3 northOffset = new Vector3();
        public Vector3 southOffset = new Vector3();
        public Vector3 eastOffset = new Vector3();
        public Vector3 westOffset = new Vector3();

        //Since it is used for drawing pawns, it is expected to be called VERY frequently. Therefore by initting this instead of converting on the fly, possible impact on performance is reduced. 
        public void Init()
        {
            northOffset = ExtractVector3(northOffsetCSV);
            southOffset = ExtractVector3(southOffsetCSV);
            eastOffset = ExtractVector3(eastOffsetCSV);
            westOffset = ExtractVector3(westOffsetCSV);
        }


        private Vector3 ExtractVector3(String extractFrom)
        {
            if (extractFrom.NullOrEmpty())
            {
                return new Vector3();
            }
            Vector3 result = new Vector3();

            List<float> values = extractFrom.Split(',').ToList().Select(x => float.Parse(x)).ToList();
            if (values.Count >= 1)
            {
                result.x = values[0];
            }
            if (values.Count >= 2)
            {
                result.y = values[1];
            }
            if (values.Count >= 3)
            {
                result.z = values[2];
            }
            return result;
        }
    }
}
