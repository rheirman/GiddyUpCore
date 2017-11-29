using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Storage
{
    public class ExtendedPawnData : IExposable
    {
        public Pawn mount = null;
        public bool hasLongNeckOrHorns = false;
        public float drawOffset = -1;

        public void ExposeData()
        {
            Scribe_References.Look(ref mount, "mount", false);
            Scribe_Values.Look(ref drawOffset, "drawOffset", 0);

        }
        public void reset()
        {
            mount = null;
            drawOffset = -1;
        }
    }
}
