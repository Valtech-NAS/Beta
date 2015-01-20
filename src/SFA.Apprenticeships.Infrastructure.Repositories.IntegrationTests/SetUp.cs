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
    public class SetUp
    {
        [SetUp]
        public void SetUpObjectFactory()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
#pragma warning restore 0618
        }
    }
}
