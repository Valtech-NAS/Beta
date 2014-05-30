namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Service;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Messaging;
    using StructureMap.Configuration.DSL;

    public class VacancyEtlRegistry : Registry
    {
        public VacancyEtlRegistry()
        {
            For<IMessageService<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            
            For<IMapper>().Use<VacancySummaryMapper>().Name = "VacancyEtl.VacancySummaryMapper";
            
            For<IIndexingService<VacancySummary>>().Use<IndexingService<VacancySummary>>();
            
            For<VacancySummaryConsumerAsync>()
                .Use<VacancySummaryConsumerAsync>()
                .Ctor<IMapper>()
                .Named("VacancyEtl.VacancySummaryMapper");

            For<IVacancySummaryProcessor>()
                .Use<VacancySummaryProcessor>()
                .Ctor<IMapper>()
                .Named("VacancyEtl.VacancySummaryMapper");

            For<VacancySchedulerConsumer>().Use<VacancySchedulerConsumer>();
        }
    }
}
