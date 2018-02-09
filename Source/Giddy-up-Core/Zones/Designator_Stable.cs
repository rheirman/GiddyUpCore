using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore.Zones
{
    class Designator_Stable : Designator
    {    
        protected Area selectedArea;


        public Designator_Stable(DesignateMode mode)
        {
            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
            this.useMouseIcon = true;
        }
        /*
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
        */
        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            if (!loc.InBounds(base.Map))
            {
                return false;
            }
            return true;
        }

        public override void ProcessInput(Event ev)
        {
            if (!base.CheckCanInteract())
            {
                return;
            }
            selectedArea = base.Map.areaManager.GetLabeled("GUC_Area_Stable");
            if (selectedArea == null)
            {
                //If no area was created yet, create one and add it to areaManager.
                selectedArea = new Area_Stable(base.Map.areaManager);
                List<Area> areaManager_areas = Traverse.Create(base.Map.areaManager).Field("areas").GetValue<List<Area>>();
                areaManager_areas.Add(selectedArea);
            }

            if (selectedArea != null)
            {
                base.ProcessInput(ev);
            }


        }

        public override void SelectedUpdate()
        {
            GenUI.RenderMouseoverBracket();
            if(selectedArea != null)
            {
                selectedArea.MarkForDraw();
            }
        }

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();
        }

    }
}
