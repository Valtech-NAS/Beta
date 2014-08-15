namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        ApplicationViewModel UpdateApplicationViewModel(Guid candidateId, ApplicationViewModel submittedApplicationViewModel);

        MyApplicationsViewModel GetMyApplications(Guid candidateId);

        void SaveApplication(Guid candidateId, int vacancyId, ApplicationViewModel applicationViewModel);

        void SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextViewModel GetSubmittedApplicationVacancySummary(Guid candidateId, int vacancyId);

        void ArchiveApplication(Guid candidateId, int vacancyId);
    }
}
