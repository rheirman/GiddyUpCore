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
    //Is used in other Giddy-up mods as a base for simple areas that can be requested in the areamanager using their label
    public class Designator_GU : Designator
    {    
        protected Area selectedArea;
        protected string areaLabel;

        public Designator_GU(DesignateMode mode)
        {
            this.soundDragSustain = SoundDefOf.DesignateDragStandard;
            this.soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
            this.useMouseIcon = true;
        }

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
            setSelectedArea(areaLabel);
            if (selectedArea != null)
            {
                base.ProcessInput(ev);
            }
        }

        protected void setSelectedArea(string areaLabel)
        {
            selectedArea = Map.areaManager.GetLabeled(areaLabel);
            if (selectedArea == null)
            {
                //If no area was created yet, create one and add it to areaManager.
                selectedArea = new Area_GU(base.Map.areaManager, areaLabel);
                List<Area> areaManager_areas = Traverse.Create(base.Map.areaManager).Field("areas").GetValue<List<Area>>();
                areaManager_areas.Add(selectedArea);
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
