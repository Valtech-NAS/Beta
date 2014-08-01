namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;

    public interface IApplicationProvider
    {
        ApplicationViewModel GetApplication(Guid applicationId);
        ApplicationViewModel GetApplicationViewModel(int vacancyId, Guid candidateId);
        void SaveApplication(ApplicationViewModel applicationViewModel);
        void SubmitApplication(Guid applicationId);

        WhatHappensNextViewModel GetSubmittedApplicationVacancySummary(Guid applicationId);
    }
}