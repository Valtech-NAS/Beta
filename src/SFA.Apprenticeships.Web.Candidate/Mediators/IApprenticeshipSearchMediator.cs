namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using ViewModels.VacancySearch;

    public interface IApprenticeshipSearchMediator
    {
        MediatorResponse<ApprenticeshipSearchResponseViewModel> Search(ApprenticeshipSearchViewModel model);
    }
}