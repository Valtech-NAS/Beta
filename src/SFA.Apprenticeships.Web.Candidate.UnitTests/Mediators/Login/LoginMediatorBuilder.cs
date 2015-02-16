namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class LoginMediatorBuilder
    {
        private Mock<IUserDataProvider> _userDataProvider;
        private Mock<ICandidateServiceProvider> _candidateServiceProvider;
        private Mock<IConfigurationManager> _configurationManager;

        public LoginMediatorBuilder()
        {
            _userDataProvider = new Mock<IUserDataProvider>();
            _configurationManager = new Mock<IConfigurationManager>();
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
        }

        public LoginMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }

        public LoginMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            return this;
        }

        public LoginMediatorBuilder With(Mock<IConfigurationManager> configurationManager)
        {
            _configurationManager = configurationManager;
            return this;
        }

        public LoginMediator Build()
        {
            var mediator = new LoginMediator(_userDataProvider.Object, _candidateServiceProvider.Object, _configurationManager.Object, new LoginViewModelServerValidator(), new AccountUnlockViewModelServerValidator(), new ResendAccountUnlockCodeViewModelServerValidator());
            return mediator;
        }
    }
}