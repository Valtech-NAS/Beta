namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.VacancyEtl;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class GatewayVacancyEtlRegistry : Registry
    {
        public GatewayVacancyEtlRegistry()
        {
            For<VacancySummaryConsumerAsync>().Use<VacancySummaryConsumerAsync>();
            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>().Use<GatewayVacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
        }
    }
}