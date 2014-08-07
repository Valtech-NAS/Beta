namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC
{
    using Domain.Interfaces.Mapping;
    using Mappers;
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
