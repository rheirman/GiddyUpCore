using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using GiddyUpCore.Utilities;
using GiddyUpCore.Storage;
using RimWorld;

namespace GiddyUpCore.Jobs
{
    public class JobDriver_Mount : JobDriver
    {
        public override bool TryMakePreToilReservations()
        {
            return true;
        }
        public Pawn Mount { get { return job.targetA.Thing as Pawn; } }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            job.canBash = true;
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnDowned(TargetIndex.A);

            yield return letMountParticipate();
            yield return Toils_General.Wait(1);//wait one tick to ensure animal is waiting to get mounted before proceding. 
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
            yield return TalkToAnimal(TargetIndex.A);
        }
        private Toil letMountParticipate()
        {
            Toil toil = new Toil();

            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.initAction = delegate
            {
                Mount.jobs.StopAll();
                Mount.pather.StopDead();
                Job jobAnimal = new Job(GUC_JobDefOf.Mounted, pawn);
                jobAnimal.count = 1;
                Mount.jobs.TryTakeOrderedJob(jobAnimal);
                ReadyForNextToil();
            };
            return toil;
        }

        private Toil TalkToAnimal(TargetIndex tameeInd)
        {
            Toil toil = new Toil();
            toil.AddFailCondition(delegate { return Mount.CurJob.def != GUC_JobDefOf.Mounted; });
            //toil.AddFailCondition(delegate { return Mount.CurJob.targetA.Thing != pawn; });
            toil.initAction = delegate
            {
                Pawn actor = toil.GetActor();
                actor.interactions.TryInteractWith(Mount, InteractionDefOf.AnimalChat);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 150;
            toil.AddFinishAction(delegate
            {
                FinishAction();
            });
            return toil;
        }

        private void FinishAction()
        {
            if (Mount.CurJob != null && Mount.CurJob.def == GUC_JobDefOf.Mounted)
            {
                ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(this.pawn);
                ExtendedPawnData animalData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Mount);
                pawnData.mount = Mount;
                TextureUtility.setDrawOffset(pawnData);
            }
        }
    }
}
