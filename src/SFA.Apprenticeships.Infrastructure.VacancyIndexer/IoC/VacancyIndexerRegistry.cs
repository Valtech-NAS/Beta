namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services;
    using StructureMap.Configuration.DSL;

    public class VacancyIndexerRegistry : Registry
    {
        public VacancyIndexerRegistry()
        {
            For<IMapper>().Use<VacancyIndexerMapper>().Name = "VacancyIndexerMapper";
            For<IVacancyIndexerService>()
                .Singleton()
                .Use<VacancyIndexerService>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");
        }
    }
}
