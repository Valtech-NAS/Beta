namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Consumers;
    using Domain.Interfaces.Messaging;
    using Messaging;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<IProcessControlQueue<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            For<MonitorSchedulerConsumer>().Use<MonitorSchedulerConsumer>();
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
                });
        }
    }
}
