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
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
            });
        }
    }
}
