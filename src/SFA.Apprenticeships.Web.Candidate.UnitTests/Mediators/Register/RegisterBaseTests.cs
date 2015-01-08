namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using Candidate.Mediators.Register;
    using Candidate.Providers;
    using Candidate.Validators;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public abstract class RegisterBaseTests
    {
        protected RegisterMediator _registerMediator;
        protected Mock<ICandidateServiceProvider> _candidateServiceProvider;
        protected ActivationViewModelServerValidator _activationViewModelServerValidator;
        protected ForgottenPasswordViewModelServerValidator _forgottenPasswordViewModelServerValidator;
        protected PasswordResetViewModelServerValidator _passwordResetViewModelServerValidator;
        protected RegisterViewModelServerValidator _registerViewModelServerValidator;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _activationViewModelServerValidator = new ActivationViewModelServerValidator();
            _forgottenPasswordViewModelServerValidator = new ForgottenPasswordViewModelServerValidator();
            _passwordResetViewModelServerValidator = new PasswordResetViewModelServerValidator();
            _registerViewModelServerValidator = new RegisterViewModelServerValidator();

            _registerMediator = new RegisterMediator(_candidateServiceProvider.Object, _registerViewModelServerValidator, _activationViewModelServerValidator, _forgottenPasswordViewModelServerValidator, _passwordResetViewModelServerValidator);
        }
    }
}
