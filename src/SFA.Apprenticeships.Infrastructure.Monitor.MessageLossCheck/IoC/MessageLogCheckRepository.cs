namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.IoC
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Monitor.Tasks;
    using Repository;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MessageLogCheckRepository : Registry
    {
        public MessageLogCheckRepository()
        {
            For<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>().Use<VacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            
            For<ICandidateDiagnosticsRepository>().Use<CandidateDiagnosticsRepository>();
            For<IApprenticeshipApplicationDiagnosticsRepository>().Use<ApprenticeshipApplicationDiagnosticsRepository>();
            For<ITraineeshipApplicationDiagnosticsRepository>().Use<TraineeshipApplicationDiagnosticsRepository>();

            For<IMessageLossCheckTaskRunner>().Use<MessageLossCheckTaskRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUnsentCandidateMessages>();
                    x.Type<CheckUnsentApprenticeshipApplicationMessages>();
                    x.Type<CheckUnsentTraineeshipApplicationMessages>();
                    //x.Type<CheckExpiredDrafts>();
                });
        }
    }
}