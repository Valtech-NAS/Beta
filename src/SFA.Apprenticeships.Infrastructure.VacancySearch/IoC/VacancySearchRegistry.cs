namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using Application.Vacancy;
    using Configuration;
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<SearchConfiguration>().Singleton().Use(SearchConfiguration.Instance);
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
        }
    }
}
