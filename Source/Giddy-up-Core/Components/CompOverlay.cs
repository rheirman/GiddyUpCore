using GiddyUpCore.Utilities;
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
            if(!(parent is Pawn))
            {
                return;
            }
            Pawn pawn = parent as Pawn;

            if (!IsMountableUtility.IsCurrentlyMounted(pawn))
            {
                return;
            }


            base.PostDraw();

            CompProperties_Overlay.GraphicOverlay overlay = Props.GetOverlay(parent.Rotation);
            if(overlay == null)
            {
                return;
            }

            Vector3 drawPos = parent.DrawPos;
            GraphicData gd;
            if(overlay.graphicDataFemale == null)
            {
                gd = overlay.graphicDataDefault;
            }
            else
            {
                gd = (pawn.gender == Gender.Female) ? overlay.graphicDataFemale : overlay.graphicDataDefault;
            }
            if (gd == null)
            {
                return;
            }
            //g.data.
            drawPos.y += 0.046875f;
            if(overlay.offsetFemale == Vector3.zero)
            {
                drawPos += overlay.offsetDefault;
            }
            else
            {
                drawPos += (pawn.gender == Gender.Female) ? overlay.offsetFemale : overlay.offsetDefault;
            }
           
            //Somehow the rotation is flipped, hence the use of GetOpposite. 
            gd.Graphic.Draw(drawPos, parent.Rotation, parent, 0f);
        }
    }

}
