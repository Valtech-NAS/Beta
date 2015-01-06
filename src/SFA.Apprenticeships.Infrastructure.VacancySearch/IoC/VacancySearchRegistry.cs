namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Configuration;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<SearchConfiguration>().Singleton().Use(SearchConfiguration.Instance);
            For<IMapper>().Use<VacancySearchMapper>().Name = "VacancySearchMapper";
            For<IVacancySearchProvider<ApprenticeshipSummaryResponse>>().Use<ApprenticeshipsSearchProvider>().Ctor<IMapper>().Named("VacancySearchMapper");
            For<IVacancySearchProvider<TraineeshipSummaryResponse>>().Use<TraineeshipsSearchProvider>().Ctor<IMapper>().Named("VacancySearchMapper");
        }
    }
}
