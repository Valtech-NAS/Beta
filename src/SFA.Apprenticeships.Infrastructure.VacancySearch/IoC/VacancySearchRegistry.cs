namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<SearchConfiguration>().Singleton().Use(SearchConfiguration.Instance);
            For<IVacancySearchProvider<ApprenticeshipSummaryResponse>>().Use<ApprenticeshipsSearchProvider>();
            For<IVacancySearchProvider<TraineeshipSummaryResponse>>().Use<TraineeshipsSearchProvider>();
        }
    }
}
