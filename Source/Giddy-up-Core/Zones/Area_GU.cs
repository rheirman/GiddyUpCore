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
    public class Area_GU : Area
    {
        private String label;
        
        private Color color = Color.magenta;

        public Area_GU()
        {

        }
        public Area_GU(AreaManager areaManager, string label) : base(areaManager)
        {
            this.color = new Color(Rand.Value, Rand.Value, Rand.Value);
            this.label = label;
        }

        public override string Label
        {
            get
            {
                return label;
            }
        }

        public override Color Color
        {
            get
            {
                return color;
            }
        }

        public override bool Mutable
        {
            get
            {
                return false;
            }
        }

        public override int ListPriority
        {
            get
            {
                return 300;
            }
        }
        public override string GetUniqueLoadID()
        {
            return label; //only one such area, so label is sufficient. 
        }
        public override bool AssignableAsAllowed(AllowedAreaMode mode)
        {
            return false;
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<string>(ref this.label, "label", null, false);
            Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
        }

    }
}
