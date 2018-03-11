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
        public Pawn caravanRider = null; //TODO: check if this can be generalized to OwnedBy without screwing up existing saves
        public Pawn ownedBy = null;
        public Pawn owning = null;
        public bool selectedForCaravan = false;
        public bool mountableByAnyone = false; //used in Giddy-up Ride and Roll
        public bool mountableByMaster = true; //used in Giddy-up Ride and Roll
        public bool wasRidingToJob = false;//used in Giddy-up Ride and Roll

        public float drawOffset = -1;
        

        public void ExposeData()
        {
            Scribe_References.Look(ref mount, "mount", false);
            Scribe_References.Look(ref caravanMount, "caravanMount", false);
            Scribe_References.Look(ref caravanRider, "caravanRider", false);
            Scribe_References.Look(ref ownedBy, "ownedBy", false);
            Scribe_References.Look(ref owning, "owning", false);
            Scribe_Values.Look(ref selectedForCaravan, "selectedForCaravan", false);
            Scribe_Values.Look(ref mountableByAnyone, "mountableByAnyone", false);
            Scribe_Values.Look(ref mountableByMaster, "mountableByMaster", true);
            Scribe_Values.Look(ref wasRidingToJob, "wasRidingToJob", false);
            Scribe_Values.Look(ref drawOffset, "drawOffset", 0);

        }
        public void reset()
        {
            mount = null;
            drawOffset = -1;
        }
    }
}
