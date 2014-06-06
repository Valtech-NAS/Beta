namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC
{
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Services;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancyIndexerRegistry : Registry
    {
        public VacancyIndexerRegistry()
        {
            For<IMapper>().Use<VacancyIndexerMapper>().Name = "VacancyIndexerMapper";
            For<IIndexerService<VacancySummaryUpdate>>()
                .Singleton()
                .Use<VacancyIndexerService>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");
        }
    }
}
