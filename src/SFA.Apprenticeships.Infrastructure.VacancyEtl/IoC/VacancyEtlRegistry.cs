namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.Common.Mappers;
    using Application.Interfaces.Messaging;
    using Application.Interfaces.Search;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Elasticsearch.Entities;
    using Elasticsearch.Service;
    using Consumers;
    using Mapper;
    using Messaging;
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
