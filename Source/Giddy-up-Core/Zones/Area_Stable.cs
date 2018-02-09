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
    public class Area_Stable : Area
    {
        private String label = Base.STABLE_LABEL;
        
        private Color color = Color.magenta;

        public Area_Stable()
        {

        }
        public Area_Stable(AreaManager areaManager) : base(areaManager)
        {
            this.color = new Color(Rand.Value, Rand.Value, Rand.Value);
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
                return true;
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<string>(ref this.label, "label", null, false);
            Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
        }

    }
}
