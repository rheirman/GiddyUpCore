using GiddyUpCore.Utilities;
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
            northOffset = TextureUtility.ExtractVector3(northOffsetCSV);
            southOffset = TextureUtility.ExtractVector3(southOffsetCSV);
            eastOffset = TextureUtility.ExtractVector3(eastOffsetCSV);
            westOffset = TextureUtility.ExtractVector3(westOffsetCSV);
        }



    }
}
