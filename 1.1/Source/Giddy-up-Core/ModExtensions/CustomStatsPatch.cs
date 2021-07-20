using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore
{
    class CustomStatsPatch : DefModExtension
    {
        public float speedModifier = 1.0f;
        public float armorModifier = 1.0f;
        public bool useMetalArmor = false;
    }
}
