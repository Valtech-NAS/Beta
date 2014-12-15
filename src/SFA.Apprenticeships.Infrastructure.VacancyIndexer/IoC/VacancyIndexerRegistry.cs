namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC
{
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Entities;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class VacancyIndexerRegistry : Registry
    {
        public VacancyIndexerRegistry()
        {
            For<IMapper>().Use<VacancyIndexerMapper>().Name = "VacancyIndexerMapper";
            For<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>()
                .Singleton()
                .Use<VacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");

            For<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>()
                .Singleton()
                .Use<VacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>()
                .Ctor<IMapper>()
                .Named("VacancyIndexerMapper");

        }
    }
}
