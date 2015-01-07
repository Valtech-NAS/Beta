namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using ViewModels.Applications;

    public interface IApprenticeshipApplicationMediator
    {
        MediatorResponse<ApprenticeshipApplicationViewModel> Resume(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationViewModel> PreviewAndSubmit(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> Save(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<AutoSaveResultViewModel> AutoSave(Guid candidateId, int vacancyId, ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyQualificationRows(ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> AddEmptyWorkExperienceRows(ApprenticeshipApplicationViewModel viewModel);

        MediatorResponse<ApprenticeshipApplicationViewModel> Preview(Guid candidateId, int vacancyId);

        MediatorResponse<ApprenticeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId);

        MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, int vacancyId, string vacancyReference, string vacancyTitle); 
    }
}