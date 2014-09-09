namespace SFA.Apprenticeships.Infrastructure.Monitor.IntegrationTests
{
    using System.Collections.Generic;
    using Common.IoC;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using NUnit.Framework;
    using Repositories.Applications;
    using Repositories.Applications.Mappers;
    using Repositories.Users;
    using Repositories.Users.Mappers;
    using StructureMap;
    using Tasks;

    [TestFixture]
    public class GivenAFailedConnectionToApplicationRepository
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Configure(c =>
            {
                c.For<IApplicationReadRepository>().Use<ApplicationRepository>();
                c.For<IMapper>().Use<ApplicationMappers>().Name = "ApplicationMapper";
                c.AddRegistry<CommonRegistry>();
            });
        }

        [TestCase]
        public void WhenMonitoringANonAccessibleApplicationRepository_ShouldReceiveAnError()
        {
            var checkApplicationRepository = ObjectFactory.GetInstance<CheckApplicationRepository>();
            var monitor = new MonitorTasksRunner(new List<IMonitorTask> {checkApplicationRepository});

            monitor.RunMonitorTasks();
        }
    }
}