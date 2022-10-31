using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GiddyUpCore.Harmony
{
    [HarmonyPatch(typeof(PawnUtility), "TrySpawnHatchedOrBornPawn")]
    class PawnUtility_TrySpawnHatchedOrBornPawn
    {
        static void Postfix(Pawn pawn, Thing motherOrEgg)
        {
            if(motherOrEgg is Pawn mother)
            {
                var pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
                var motherData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(mother);
                pawnData.mountableByAnyone = motherData.mountableByAnyone;
                pawnData.mountableByMaster = motherData.mountableByMaster;
            }
        }
    }
}
