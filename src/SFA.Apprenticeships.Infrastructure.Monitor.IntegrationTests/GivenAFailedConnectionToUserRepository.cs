namespace SFA.Apprenticeships.Infrastructure.Monitor.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using Common.IoC;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using NLog;
    using NUnit.Framework;
    using Repositories.Candidates;
    using Repositories.Users;
    using Repositories.Users.Mappers;
    using StructureMap;
    using Tasks;

    [TestFixture]
    public class GivenAFailedConnectionToUserRepository
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Configure(c =>
            {
                c.For<IUserReadRepository>().Use<UserRepository>();
                c.For<IMapper>().Use<UserMappers>().Name = "UserMapper";
                c.AddRegistry<CommonRegistry>();
            });
        }

        [TestCase]
        public void WhenMonitoringANonAccessibleUserRepository_ShouldReceiveAnError()
        {
            var checkUserRepositoryTask = ObjectFactory.GetInstance<CheckUserRepository>();
            var monitor = new MonitorTasksRunner(new List<IMonitorTask> { checkUserRepositoryTask });

            monitor.RunMonitorTasks();
        }
    }
}
