using GiddyUpCore.Jobs;
using GiddyUpCore.Storage;
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

        public static bool isMountable(Pawn pawn)
        {
            return isMountable(pawn, out Reason reason);
        }

        public static bool IsCurrentlyMounted(Pawn animal)
        {
            if(animal.CurJob == null || animal.CurJob.def != GUC_JobDefOf.Mounted)
            {
                return false;
            }
            JobDriver_Mounted mountedDriver = (JobDriver_Mounted)animal.jobs.curDriver;
            Pawn Rider = mountedDriver.Rider;
            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();
            if(store == null || store.GetExtendedDataFor(Rider).mount == null)
            {
                return false;
            }
            return true;
        }

        public static bool isMountable(Pawn pawn, out Reason reason)
        {
            reason = Reason.CanMount;
            if (!isAllowedInModOptions(pawn.def.defName))
            {
                reason = Reason.NotInModOptions;
                return false;
            }
            if (pawn.ageTracker.CurLifeStageIndex != pawn.RaceProps.lifeStageAges.Count - 1)
            {
                if (!pawn.def.HasModExtension<AllowedLifeStagesPatch>())
                {
                    reason = Reason.NotFullyGrown;
                    return false;
                }
                else //Use custom life stages instead of last life stage if a patch exists for that
                {
                    AllowedLifeStagesPatch customLifeStages = pawn.def.GetModExtension<AllowedLifeStagesPatch>();
                    if (!customLifeStages.getAllowedLifeStagesAsList().Contains(pawn.ageTracker.CurLifeStageIndex))
                    {
                        reason = Reason.NotFullyGrown;
                        return false;
                    }
                }
            }
            if (pawn.training == null || (pawn.training != null && !pawn.training.IsCompleted(TrainableDefOf.Obedience)))
            {
                reason = Reason.NeedsObedience;
                return false;
            }
            return true;
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
