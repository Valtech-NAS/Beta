namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;

    public interface ITraineeshipApplicationProvider
    {
        TraineeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        TraineeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId);
    }
}