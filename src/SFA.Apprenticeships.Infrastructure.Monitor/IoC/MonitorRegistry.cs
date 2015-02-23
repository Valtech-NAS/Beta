namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Common.Configuration;
    using Consumers;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    using Repositories;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<MonitorControlQueueConsumer>().Use<MonitorControlQueueConsumer>();
            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>();
            For<IConfigurationManager>().Use<ConfigurationManager>();

            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUserRepository>();
                    x.Type<CheckApprenticeshipApplicationRepository>();
                    x.Type<CheckTraineeshipApplicationRepository>();
                    x.Type<CheckCandidateRepository>();
                    x.Type<CheckVacancySearch>();
                    x.Type<CheckLocationLookup>();
                    x.Type<CheckAddressSearch>();
                    x.Type<CheckPostcodeService>();
                    x.Type<CheckUserDirectory>();
                    x.Type<CheckRabbitMessageQueue>();
                    x.Type<CheckNasGateway>();
                    x.Type<CheckMongoReplicaSets>();
                    x.Type<CheckElasticsearchCluster>();
                    x.Type<CheckElasticsearchAliases>();
                    x.Type<CheckLogstashLogs>();
                });

            For<IDailyMetricsTasksRunner>().Use<DailyMetricsTasksRunner>()
                .EnumerableOf<IDailyMetricsTask>()
                .Contains(x => x.Type<SendDailyMetricsEmail>());

            For<IMongoAdminClient>().Use<MongoAdminClient>();
            For<IApprenticeshipMetricsRepository>().Use<ApprenticeshipMetricsRepository>();
            For<ICommunicationMetricsRepository>().Use<CommunicationMetricsRepository>();
            For<ITraineeshipMetricsRepository>().Use<TraineeshipMetricsRepository>();
            For<IUserMetricsRepository>().Use<UserMetricsRepository>();
        }
    }
}