namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.Interfaces.Messaging;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Mapping;
    using Consumers;
    using Messaging;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper;
    using StructureMap.Configuration.DSL;

    public class VacancyEtlRegistry : Registry
    {
        public VacancyEtlRegistry()
        {
            For<IMessageService<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            
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
