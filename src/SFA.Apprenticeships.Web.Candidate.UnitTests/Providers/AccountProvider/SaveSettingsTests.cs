namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Domain.Entities.Candidates;
    using Application.Interfaces.Candidates;
    using Builders;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveSettingsTests
    {
        [TestCase("0123456789", false, false)]
        [TestCase("0123456789", true, false)]
        [TestCase("0123456789", false, true)]
        [TestCase("0123456789", true, true)]
        public void MappingTest(string phoneNumber, bool allowEmailComms, bool allowSmsComms)
        {
            Candidate candidate = null;
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).Build);
            candidateService.Setup(cs => cs.SaveCandidate(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(phoneNumber).AllowEmailComms(allowEmailComms).AllowSmsComms(allowSmsComms).Build();
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var result = provider.SaveSettings(candidateId, viewModel);

            result.Should().BeTrue();
            candidate.RegistrationDetails.Should().NotBeNull();
            candidate.RegistrationDetails.PhoneNumber.Should().Be(phoneNumber);
            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.AllowEmail.Should().Be(allowEmailComms);
            candidate.CommunicationPreferences.AllowMobile.Should().Be(allowSmsComms);
            candidate.CommunicationPreferences.VerifiedMobile.Should().BeFalse();
        }
    }
}