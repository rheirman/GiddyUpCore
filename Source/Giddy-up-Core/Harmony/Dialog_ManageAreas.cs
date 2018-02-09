using GiddyUpCore.Zones;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace GiddyUpCore.Harmony
{
    /*
    [HarmonyPatch(typeof(Dialog_ManageAreas), "DoWindowContents")]
    class Dialog_ManageAreas_DoWindowContents
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            for (var i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];
                yield return instruction;
                if (i < instructionsList.Count - 1 && instructionsList[i + 1].operand == typeof(Listing).GetMethod("End"))
                {
                    //We have to insert here, otherwise the code branching will be messed up. ldloc is already called here. 
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, typeof(Dialog_ManageAreas_DoWindowContents).GetMethod("addStableAreaButton"));//Injected code     
                    yield return new CodeInstruction(OpCodes.Ldloc_0); //Next call excpects a Ldloc_0, previous one already taken off the stack. 
                }
            }
        }

        public static void addStableAreaButton(Listing_Standard listing, Dialog_ManageAreas instance)
        {
            Map map = Traverse.Create(instance).Field("map").GetValue<Map>();
            if (listing.ButtonText("GUC_NewStableArea".Translate(), null))
            {
                Area_Stable area_stable;
                map.areaManager.TryMakeNewStableArea(AllowedAreaMode.Animal, out area_stable);
            }
        }
    }
    */
}
