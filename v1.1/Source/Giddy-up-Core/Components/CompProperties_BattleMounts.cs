using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore
{
    class CompProperties_Mount : CompProperties
    {
        public CompProperties_Mount()
        {
            compClass = typeof(CompMount);
        }

        public bool drawFront = false;
        public bool isException = false;
    }
}
