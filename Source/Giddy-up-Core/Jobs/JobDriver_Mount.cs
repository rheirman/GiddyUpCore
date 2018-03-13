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
            //For automatic mounting, reserve the mount aswell as targets of the job the pawn is riding to (target B and possibly C). 
            if (job.count == -1)
            {
                job.count = 1;
            }
            int stackCount = -1;
            if (job.count > 1)
            {
                stackCount = 0;
            }
            if (!this.job.targetQueueA.NullOrEmpty())
            {
                this.pawn.ReserveAsManyAsPossible(this.job.targetQueueA, this.job, 1, -1, null);
            }
            if (!this.job.targetQueueB.NullOrEmpty())
            {
                this.pawn.ReserveAsManyAsPossible(this.job.targetQueueA, this.job, 1, -1, null);
            }
            if (this.job.targetB != null && this.job.targetC != null)
            {
                return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null);
            }
            else if (this.job.targetB != null)
            {
                return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, this.job.count, stackCount, null);
            }
            return true;
        }
        private Pawn Mount { get { return job.targetA.Thing as Pawn; } }
        
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
            toil.initAction = delegate
            {
                Pawn actor = toil.GetActor();
                actor.interactions.TryInteractWith(Mount, InteractionDefOf.AnimalChat);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Delay;
            toil.defaultDuration = 150;
            toil.AddFinishAction(delegate {
                if (Mount.CurJob != null && Mount.CurJob.def == GUC_JobDefOf.Mounted)
                {
                    Pawn actor = toil.GetActor();
                    ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(actor);
                    ExtendedPawnData animalData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(Mount);
                    pawnData.owning = Mount;
                    animalData.ownedBy = pawn;
                    pawnData.mount = (Pawn)((Thing)actor.CurJob.GetTarget(tameeInd));
                    TextureUtility.setDrawOffset(pawnData);
                }
            });
            return toil;
        }

    }
}
