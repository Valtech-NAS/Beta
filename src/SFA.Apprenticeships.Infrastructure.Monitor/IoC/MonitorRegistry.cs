namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Consumers;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Messaging;
    using Repositories.Users;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<IProcessControlQueue<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            For<MonitorSchedulerConsumer>().Use<MonitorSchedulerConsumer>();
            For<MonitorTasksRunner>().Use<MonitorTasksRunner>();
            For<CheckUserRepository>().Use<CheckUserRepository>();
            For<IUserReadRepository>().Use<UserRepository>();

            //For<IEnumerable<IMonitorTask> >().Use()
        }
    }
}
