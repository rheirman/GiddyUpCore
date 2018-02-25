using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GiddyUpCore.Utilities
{
    public class CanMountUtility
    {
        public enum Reason{NotFullyGrown, NeedsObedience, NotInModOptions, CanMount};
        public static bool canMount(Pawn animal, out Reason reason)
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
            bool found = GiddyUpCore.Base.animalSelecter.Value.InnerList.TryGetValue(animal.def.defName, out AnimalRecord value);
            if (found && !value.isSelected)
            {
                reason = Reason.NotInModOptions;
                canMount = false;
            }
            return canMount;
        }
    }
}
