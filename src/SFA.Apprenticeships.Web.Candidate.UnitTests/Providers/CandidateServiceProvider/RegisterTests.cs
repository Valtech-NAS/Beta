namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RegisterTests
    {
        [Test]
        public void Us616_Ac1_DefaultCommunicationPreferencesEmailOnly()
        {
            Candidate candidate = null;
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.Register(It.IsAny<Candidate>(), It.IsAny<string>())).Callback<Candidate, string>((c, s) => { candidate = c; });
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new RegisterViewModelBuilder().Build();

            var registered = provider.Register(viewModel);

            candidate.Should().NotBeNull();
            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.AllowEmail.Should().BeTrue();
            candidate.CommunicationPreferences.AllowMobile.Should().BeFalse();
            candidate.CommunicationPreferences.VerifiedMobile.Should().BeFalse();
            registered.Should().BeTrue();
        }
    }
}