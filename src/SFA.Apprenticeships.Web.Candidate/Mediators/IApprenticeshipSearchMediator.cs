namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchViewModel> Index();

        MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model);

        MediatorResponse<VacancyDetailViewModel> Details(int vacancyId, Guid? candidateId);
    }
}