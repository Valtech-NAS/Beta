namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface IApprenticeshipApplicationProvider
    {
        ApprenticeshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel GetOrCreateApplicationViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApprenticeshipApplicationViewModel savedModel, ApprenticeshipApplicationViewModel submittedModel);

        MyApplicationsViewModel GetMyApplications(Guid candidateId);

        void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel apprenticeshipApplicationViewModel);

        ApprenticeshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId);

        ApprenticeshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId);

        TraineeshipFeatureViewModel GetTraineeshipFeatureViewModel(Guid candidateId);
    }
}
