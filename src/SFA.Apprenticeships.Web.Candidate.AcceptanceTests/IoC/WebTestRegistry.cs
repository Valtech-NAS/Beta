namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.IoC
{
    using Infrastructure.Common.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.UserDirectory.IoC;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class WebTestRegistry
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
            });
#pragma warning restore 0618
        }
    }
}
