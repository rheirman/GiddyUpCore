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
        public GraphicOverlay overlayFront;
        public GraphicOverlay overlaySide;
        public GraphicOverlay overlayBack;

        public class GraphicOverlay
        {
            public GraphicData graphicDataDefault;
            public GraphicData graphicDataFemale;
            public GraphicData graphicDataMale;

            public Vector3 offsetDefault = Vector3.zero;
            public Vector3 offsetFemale = Vector3.zero;
            public Vector3 offsetMale = Vector3.zero;

            public List<GraphicData> allVariants;

        }
        public GraphicOverlay GetOverlay(Rot4 dir)
        {
            if (dir == Rot4.South)
            {
                return overlayFront;
            }
            if (dir == Rot4.North)
            {
                return overlayBack;
            }
            return overlaySide;
        }

        public CompProperties_Overlay()
        {
            compClass = typeof(CompOverlay);
        }
    }
}
