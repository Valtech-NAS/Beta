namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
        }
    }
}
