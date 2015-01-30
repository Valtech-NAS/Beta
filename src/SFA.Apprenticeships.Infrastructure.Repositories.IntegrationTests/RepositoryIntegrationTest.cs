namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using Common.IoC;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using Repositories.Users.IoC;
    using StructureMap;

    [SetUpFixture]
    public class RepositoryIntegrationTest
    {
        protected Container Container;

        [SetUp]
        public void SetUpObjectFactory()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }
    }
}
