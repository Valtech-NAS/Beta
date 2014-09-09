namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using Application.Interfaces.Locations;
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
            For<CheckCandidateRepository>().Use<CheckCandidateRepository>();
            For<CheckApplicationRepository>().Use<CheckApplicationRepository>();
            For<CheckVacancySearch>().Use<CheckVacancySearch>();
            For<CheckLocationLookup>().Use<CheckLocationLookup>();

            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUserRepository>();
                    x.Type<CheckApplicationRepository>();
                    x.Type<CheckCandidateRepository>();
                    x.Type<CheckVacancySearch>();
                    x.Type<CheckLocationLookup>();
                });
        }
    }
}
