using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Utilities
{
    public static class IsMountableUtility
    {
        public enum Reason{NotFullyGrown, NeedsObedience, NotInModOptions, CanMount};
        public static bool isMountable(Pawn animal, out Reason reason)
        {
            bool canMount = true;
            reason = Reason.CanMount;
            if (animal.ageTracker.CurLifeStageIndex != animal.RaceProps.lifeStageAges.Count - 1)
            {
                reason = Reason.NotFullyGrown;
                canMount = false;
            }
            if (!(animal.training != null && animal.training.IsCompleted(TrainableDefOf.Obedience)))
            {
                reason = Reason.NeedsObedience;
                canMount = false;
            }
            if (!isAllowedInModOptions(animal.def.defName))
            {
                reason = Reason.NotInModOptions;
                canMount = false;
            }
            return canMount;
        }

        public static bool isAllowedInModOptions(String animalName)
        {
            GiddyUpCore.AnimalRecord value;
            bool found = GiddyUpCore.Base.animalSelecter.Value.InnerList.TryGetValue(animalName, out value);
            if (found && value.isSelected)
            {
                return true;
            }
            return false;
        }
    }
}
