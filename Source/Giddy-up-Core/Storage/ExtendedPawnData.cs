using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

//Note: Currently this class contains information specific for other mods (caravanMount, caravanRider, etc), which is of course not ideal for a core framework. Ideally it should be completely generic. However I have yet to come up with an
// way to do this properly without introducing a lot of extra work. So for now I'll just keep it as it is. 

namespace GiddyUpCore.Storage
{
    public class ExtendedPawnData : IExposable
    {
        public Pawn mount = null;
        public Pawn caravanMount = null;
        public Pawn caravanRider = null;
        public bool selectedForCaravan = false;
        public float drawOffset = -1;
        

        public void ExposeData()
        {
            Scribe_References.Look(ref mount, "mount", false);
            Scribe_References.Look(ref caravanMount, "caravanMount", false);
            Scribe_References.Look(ref caravanRider, "caravanRider", false);
            Scribe_Values.Look(ref selectedForCaravan, "selectedForCaravan", false);
            Scribe_Values.Look(ref drawOffset, "drawOffset", 0);

        }
        public void reset()
        {
            mount = null;
            drawOffset = -1;
        }
    }
}
