using SFA.Apprenticeships.Infrastructure.Mongo.Common;

namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Consumers;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
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
                    //x.Type<CheckActiveDirectory>();
                    x.Type<CheckUserDirectory>();
                    x.Type<CheckRabbitMessageQueue>();
                    x.Type<CheckNasGateway>();
                    x.Type<CheckMongoReplicaSets>();
                    x.Type<CheckElasticsearchCluster>();
                    x.Type<CheckElasticsearchAliases>();
                    x.Type<CheckLogstashLogs>();
                });

            For<IMongoAdminClient>().Use<MongoAdminClient>();
        }
    }
}
