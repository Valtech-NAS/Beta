namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using Application.Vacancy;
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
        }
    }
}
