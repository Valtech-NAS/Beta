namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using Entities;

    public interface IVacancyIndexDataProvider
    {
        int GetVacancyPageCount();

        VacancySummaries GetVacancySummaries(int pageNumber);
    }
}
