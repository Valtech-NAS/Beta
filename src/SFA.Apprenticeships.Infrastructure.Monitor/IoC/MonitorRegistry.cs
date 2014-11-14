using SFA.Apprenticeships.Infrastructure.Mongo.Common;

namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Consumers;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<MonitorControlQueueConsumer>().Use<MonitorControlQueueConsumer>();
            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>();

            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUserRepository>();
                    x.Type<CheckApplicationRepository>();
                    x.Type<CheckCandidateRepository>();
                    x.Type<CheckVacancySearch>();
                    x.Type<CheckLocationLookup>();
                    x.Type<CheckAddressSearch>();
                    x.Type<CheckPostcodeService>();
                    x.Type<CheckActiveDirectory>();
                    x.Type<CheckRabbitMessageQueue>();
                    x.Type<CheckNasGateway>();
                    x.Type<CheckMongoReplicaSets>();
                    x.Type<CheckElasticsearchCluster>();
                    x.Type<CheckElasticsearchAliases>();
                });

            For<IMongoAdminClient>().Use<MongoAdminClient>();
        }
    }
}
