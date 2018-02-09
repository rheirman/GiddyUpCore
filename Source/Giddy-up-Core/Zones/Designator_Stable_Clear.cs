using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Zones
{
    class Designator_Stable_Clear : Designator_Stable
    {

        public Designator_Stable_Clear() : base(DesignateMode.Remove)
        {
            defaultLabel = "GUC_Designator_Stable_Clear_Label".Translate();
            defaultDesc = "GUC_Designator_Stable_Clear_Description".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/GUC_Designator_Stable_Clear", true);
        }
        public override void DesignateSingleCell(IntVec3 c)
        {
            selectedArea[c] = false;
        }

        public override int DraggableDimensions
        {
            get
            {
                return 2;
            }
        }

        public override bool DragDrawMeasurements
        {
            get
            {
                return true;
            }
        }


        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            return c.InBounds(base.Map) && selectedArea != null && selectedArea[c];
        }

    }
}
