namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.Interfaces.ReferenceData;
    using Application.ReferenceData;
    using Application.VacancyEtl;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class GatewayVacancyEtlRegistry : Registry
    {
        public GatewayVacancyEtlRegistry()
        {
            For<ApprenticeshipSummaryUpdateConsumerAsync>().Use<ApprenticeshipSummaryUpdateConsumerAsync>();
            For<TraineeshipsSummaryUpdateConsumerAsync>().Use<TraineeshipsSummaryUpdateConsumerAsync>();
            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>()
                .Use<VacancySummaryProcessor>()
                .Ctor<IMapper>()
                .Named("VacancyEtlMapper");
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
            For<VacancyAboutToExpireConsumerAsync>()
                .Use<VacancyAboutToExpireConsumerAsync>()
                .Ctor<IMapper>()
                .Named("VacancyEtlMapper");

            For<IReferenceDataService>().Use<ReferenceDataService>();
        }
    }
}