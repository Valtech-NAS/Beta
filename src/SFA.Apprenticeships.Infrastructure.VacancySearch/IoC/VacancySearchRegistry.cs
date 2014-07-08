namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.Vacancies;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
        }
    }
}
