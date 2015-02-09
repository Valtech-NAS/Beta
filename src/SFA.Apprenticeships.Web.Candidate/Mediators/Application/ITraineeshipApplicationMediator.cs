namespace SFA.Apprenticeships.Web.Candidate.Mediators.Application
{
    using System;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationMediator
    {
        MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, string vacancyIdString);

        MediatorResponse<TraineeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyQualificationRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyWorkExperienceRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, int vacancyId, string vacancyReference, string vacancyTitle);
    }
}