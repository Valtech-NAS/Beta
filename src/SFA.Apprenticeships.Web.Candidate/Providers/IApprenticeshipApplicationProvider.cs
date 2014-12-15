namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Applications;
    using ViewModels.MyApplications;

    public interface IApprenticeshipApplicationProvider
    {
        ApprenticheshipApplicationViewModel GetApplicationViewModel(Guid candidateId, int vacancyId);

        ApprenticheshipApplicationViewModel PatchApplicationViewModel(Guid candidateId, ApprenticheshipApplicationViewModel savedModel, ApprenticheshipApplicationViewModel submittedModel);

        MyApplicationsViewModel GetMyApplications(Guid candidateId);

        void SaveApplication(Guid candidateId, int vacancyId, ApprenticheshipApplicationViewModel apprenticheshipApplicationViewModel);

        ApprenticheshipApplicationViewModel SubmitApplication(Guid candidateId, int vacancyId);

        WhatHappensNextViewModel GetWhatHappensNextViewModel(Guid candidateId, int vacancyId);

        ApprenticheshipApplicationViewModel ArchiveApplication(Guid candidateId, int vacancyId);

        ApprenticheshipApplicationViewModel DeleteApplication(Guid candidateId, int vacancyId);
    }
}
