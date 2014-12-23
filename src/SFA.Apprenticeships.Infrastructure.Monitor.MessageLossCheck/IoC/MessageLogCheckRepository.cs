namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.IoC
{
    using Monitor.Tasks;
    using Repository;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MessageLogCheckRepository : Registry
    {
        public MessageLogCheckRepository()
        {
            For<ICandidateDiagnosticsRepository>().Use<CandidateDiagnosticsRepository>();
            For<IApprenticeshipApplicationDiagnosticsRepository>().Use<ApprenticeshipApplicationDiagnosticsRepository>();
            For<ITraineeshipApplicationDiagnosticsRepository>().Use<TraineeshipApplicationDiagnosticsRepository>();

            For<IMessageLossCheckTaskRunner>().Use<MessageLossCheckTaskRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUnsentCandidateMessages>();
                    x.Type<CheckUnsentApplicationMessages>();
                });
        }
    }
}