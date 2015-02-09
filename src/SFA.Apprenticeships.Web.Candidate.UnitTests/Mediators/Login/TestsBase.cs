namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected ILoginMediator Mediator;

        [SetUp]
        public void Setup()
        {
            UserDataProvider = new Mock<IUserDataProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            Mediator = new LoginMediator(UserDataProvider.Object, CandidateServiceProvider.Object, ConfigurationManager.Object,
                new LoginViewModelServerValidator(), new AccountUnlockViewModelServerValidator(), new ResendAccountUnlockCodeViewModelServerValidator());
        }
    }
}