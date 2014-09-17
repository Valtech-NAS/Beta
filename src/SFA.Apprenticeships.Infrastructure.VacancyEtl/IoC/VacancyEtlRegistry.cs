namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.VacancyEtl;
    using Domain.Interfaces.Mapping;
    using Consumers;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancyEtlRegistry : Registry
    {
        public VacancyEtlRegistry()
        {
            For<VacancySummaryConsumerAsync>().Use<VacancySummaryConsumerAsync>();
            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
        }
    }
}
