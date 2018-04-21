using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore
{
    public class CompProperties_Overlay : CompProperties
    {
        public GraphicOverlay overlay;

        public class GraphicOverlay
        {
            public GraphicData graphicData;
            public Vector3 offset = Vector3.zero;
        }

        public CompProperties_Overlay()
        {
            compClass = typeof(CompOverlay);
        }
    }
}
