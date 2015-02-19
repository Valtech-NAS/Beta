namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationProvider
    {
        TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel traineeshipApplicationViewModel);

        WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId,
            TraineeshipApplicationViewModel savedModel, TraineeshipApplicationViewModel submittedModel);
    }
}
