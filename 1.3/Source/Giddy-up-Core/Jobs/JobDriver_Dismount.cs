using GiddyUpCore.Storage;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace GiddyUpCore.Jobs
{
    class JobDriver_Dismount : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
            
            //yield return TalkToAnimal(pawnData.mount);
            yield return dismount();
        }
        private Toil TalkToAnimal(Pawn animal)
        {
            Toil toil = new Toil();
            toil.AddFailCondition(delegate { return animal.CurJob.def != GUC_JobDefOf.Mounted; });
            toil.initAction = delegate
            {
                Pawn actor = toil.GetActor();
                actor.interactions.TryInteractWith(animal, InteractionDefOf.AnimalChat);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 150;
            return toil;
        }

        private Toil dismount()
        {
            ExtendedDataStorage store = Base.Instance.GetExtendedDataStorage();

            Toil toil = new Toil();
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.initAction = delegate {
                if(store == null)
                {
                    ReadyForNextToil();
                    return;
                }
                ExtendedPawnData pawnData = store.GetExtendedDataFor(pawn);
                if(pawnData.mount != null)
                {
                    pawnData.mount.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
                ReadyForNextToil();
            };
            return toil;

        }
    }
}
