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
            For<CheckUserRepository>().Use<CheckUserRepository>();

            //For<IEnumerable<IMonitorTask> >().Use()
        }
    }
}
