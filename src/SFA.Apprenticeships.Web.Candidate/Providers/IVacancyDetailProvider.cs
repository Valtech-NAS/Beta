namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.VacancySearch;

    public interface IVacancyDetailProvider
    {
        VacancyDetailViewModel GetVacancyDetailViewModel(int id);
    }
}