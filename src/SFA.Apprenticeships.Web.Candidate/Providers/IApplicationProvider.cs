namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        ApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApplicationViewModel savedModel, ApplicationViewModel submittedModel);

        MyApplicationsViewModel GetMyApplications(Guid candidateId);

        void SaveApplication(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel);

        void SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        void ArchiveApplication(Guid candidateId, int vacancyId);
    }
}
