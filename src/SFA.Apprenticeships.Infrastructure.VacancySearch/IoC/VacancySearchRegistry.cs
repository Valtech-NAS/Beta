using SFA.Apprenticeships.Application.Interfaces.Vacancies;

namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IoC
{
    using StructureMap.Configuration.DSL;

    public class VacancySearchRegistry : Registry
    {
        public VacancySearchRegistry()
        {
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
        }
    }
}
