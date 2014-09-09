namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using System.Collections.Generic;
    using Application.Vacancy;
    using Consumers;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Messaging;
    using Repositories.Applications;
    using Repositories.Candidates;
    using Repositories.Users;
    using StructureMap.Configuration.DSL;
    using Tasks;
    using VacancySearch;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<IProcessControlQueue<StorageQueueMessage>>().Use<AzureScheduleQueue>();
            For<MonitorSchedulerConsumer>().Use<MonitorSchedulerConsumer>();
            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>();
            For<CheckUserRepository>().Use<CheckUserRepository>();
            For<IUserReadRepository>().Use<UserRepository>();
            For<ICandidateReadRepository>().Use<CandidateRepository>();
            For<IApplicationReadRepository>().Use<ApplicationRepository>();
            For<IVacancySearchProvider>().Use<VacancySearchProvider>();
            For<IMonitorTask>().Use<CheckUserRepository>();
            For<IMonitorTask>().Use<CheckCandidateRepository>();
            For<IMonitorTask>().Use<CheckApplicationRepository>();
            For<IMonitorTask>().Use<CheckVacancySearch>();
            //For<IEnumerable<IMonitorTask>>().Use(new List<IMonitorTask>
            //{

            //});
        }
    }
}
