namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchViewModel> Index(ApprenticeshipSearchMode searchMode);

        MediatorResponse<ApprenticeshipSearchViewModel> SearchValidation(ApprenticeshipSearchViewModel model);

        MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model);

        MediatorResponse<VacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId);
    }
}