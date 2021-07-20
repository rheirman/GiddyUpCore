using GiddyUpCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (!(parent is Pawn))
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

            


            gd = (pawn.gender == Gender.Female) ? overlay.graphicDataFemale : overlay.graphicDataMale;
            if(gd == null)
            {
                gd = overlay.graphicDataDefault;
            }
            if (gd == null)
            {
                return;
            }

            //support multi texture animals
            if(overlay.allVariants != null)
            {
                string graphicPath = pawn.Drawer.renderer.graphics.nakedGraphic.path;
                string graphicName = graphicPath.Split('/').Last();
                bool foundTex = false;
                foreach (var variant in overlay.allVariants)
                {
                    
                    string variantName = variant.texPath.Split('/').Last().Split(overlay.stringDelimiter.ToCharArray()).First();
                    if (graphicName == variantName)
                    {
                        //set required properties
                        string texPath = variant.texPath;
                        variant.CopyFrom(gd);
                        variant.texPath = texPath;
                        gd = variant;
                        foundTex = true;
                    }
                }
                if (!foundTex)
                {
                    //Don't throw errors when there's no valid texture. 
                    return;
                }
            }


            //g.data.
            drawPos.y += 0.046875f;
            Vector3 offset = (pawn.gender == Gender.Female) ? overlay.offsetFemale : overlay.offsetMale;
            if(offset == Vector3.zero)
            {
                offset = overlay.offsetDefault;
            }
            if(pawn.Rotation == Rot4.West)
            {
                offset.x = -offset.x;
            }

            drawPos += offset;

            //Somehow the rotation is flipped, hence the use of GetOpposite. 
            gd.Graphic.Draw(drawPos, parent.Rotation, parent, 0f);
        }
    }

}
