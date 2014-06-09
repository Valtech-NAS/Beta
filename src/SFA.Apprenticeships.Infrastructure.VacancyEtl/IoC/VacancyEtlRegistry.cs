namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Mapping;
    using Consumers;
    using Messaging;
    using Mapper;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancyEtlRegistry : Registry
    {
        public VacancyEtlRegistry()
        {
            For<IProcessControlQueue<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            
            For<VacancySummaryConsumerAsync>().Use<VacancySummaryConsumerAsync>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";

            For<IVacancySummaryProcessor>()
                .Use<VacancySummaryProcessor>()
                .Ctor<IMapper>()
                .Named("VacancyEtlMapper");

            For<VacancySchedulerConsumer>().Use<VacancySchedulerConsumer>();
        }
    }
}
