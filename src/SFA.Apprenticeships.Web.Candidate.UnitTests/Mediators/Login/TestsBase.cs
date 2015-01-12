namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected ILoginMediator Mediator;

        [SetUp]
        public void Setup()
        {
            UserDataProvider = new Mock<IUserDataProvider>();
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            Mediator = new LoginMediator(UserDataProvider.Object, CandidateServiceProvider.Object,
                new LoginViewModelServerValidator(), new AccountUnlockViewModelServerValidator(), new ResendAccountUnlockCodeViewModelServerValidator());
        }
    }
}