using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GiddyUpCore
{
    class CompOverlay : ThingComp
    {
        public CompProperties_Overlay Props => props as CompProperties_Overlay;
        public override void PostDraw()
        {
            base.PostDraw();
            if(parent.Rotation == Rot4.South)
            {
                Vector3 drawPos = parent.DrawPos;
                Graphic g = Props.overlay.graphicData.Graphic;
                //g.data.
                drawPos.y += 0.046875f;
                drawPos += Props.overlay.offset;

                Props.overlay.graphicData.Graphic.Draw(drawPos, Rot4.North, parent, 0f);
            }

        }
    }
}
