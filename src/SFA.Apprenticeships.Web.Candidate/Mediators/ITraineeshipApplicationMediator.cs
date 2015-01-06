namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationMediator
    {
        MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId);
    }
}