namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using System.Web.Mvc;
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchViewModel> Index();

        MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(ApprenticeshipSearchViewModel model, ModelStateDictionary modelState);

        MediatorResponse<VacancyDetailViewModel> Details(int id, Guid? candidateId, string searchReturnUrl);
    }
}