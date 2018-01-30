using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Zones
{
    class Designator_Stable_Expand : Designator_Stable
    {

        public Designator_Stable_Expand() : base(DesignateMode.Add)
        {
            defaultLabel = "GUC_Designator_Stable_Add_Label".Translate();
            defaultDesc = "GUC_Designator_Stable_Add_Description".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/GUC_Designator_Stable_Add", true);
        }

        //public override AcceptanceReport CanDesignateCell(IntVec3 c)
        //{
        //    return c.InBounds(base.Map) && Designator_Stable.SelectedArea != null && Designator_Stable.SelectedArea[c];
        //}
        public override void DesignateSingleCell(IntVec3 c)
        {
            selectedArea[c] = true;
        }
        public override AcceptanceReport CanDesignateCell(IntVec3 c)
        {
            return c.InBounds(base.Map) && selectedArea != null && !selectedArea[c];
        }


    }
}
