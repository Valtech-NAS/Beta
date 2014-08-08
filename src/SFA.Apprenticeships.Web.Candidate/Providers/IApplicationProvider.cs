namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplicationViewModel(Guid applicationViewId);
        ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId);
        ApplicationViewModel GetApplicationViewModel(ApplicationViewModel submittedApplicationViewModel);
        void SaveApplication(ApplicationViewModel applicationViewModel);
        void SubmitApplication(Guid applicationId);
        WhatHappensNextViewModel GetSubmittedApplicationVacancySummary(Guid applicationId);
        MyApplicationsViewModel GetMyApplications(Guid candidateId);
    }
}