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
    using Repositories.Candidates.Mappers;
    using Repositories.Users;
    using Repositories.Users.Mappers;
    using StructureMap;
    using Tasks;

    [TestFixture]
    public class GivenAFailedConnectionToCandidateRepository
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Configure(c =>
            {
                c.For<ICandidateReadRepository>().Use<CandidateRepository>();
                c.For<IMapper>().Use<CandidateMappers>().Name = "CandidateMapper";
                c.AddRegistry<CommonRegistry>();
            });
        }

        [TestCase]
        public void WhenMonitoringANonAccessibleCandidateRepository_ShouldReceiveAnError()
        {
            var checkCandidateRepository = ObjectFactory.GetInstance<CheckCandidateRepository>();
            var monitor = new MonitorTasksRunner(new List<IMonitorTask> { checkCandidateRepository });

            monitor.RunMonitorTasks();
        }
    }
}
