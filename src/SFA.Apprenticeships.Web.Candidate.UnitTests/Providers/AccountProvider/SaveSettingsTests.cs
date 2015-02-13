namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Domain.Entities.Candidates;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveSettingsTests
    {
        [TestCase("0123456789", false, false, false)]
        [TestCase("0123456789", false, true, false)]
        [TestCase("0123456789", false, false, true)]
        [TestCase("0123456789", false, true, true)]
        [TestCase("0123456789", true, false, false)]
        [TestCase("0123456789", true, true, false)]
        [TestCase("0123456789", true, false, true)]
        [TestCase("0123456789", true, true, true)]
        public void MappingTest(string phoneNumber, bool verifiedMobile, bool allowEmailComms, bool allowSmsComms)
        {
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).PhoneNumber(phoneNumber).Build);
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(phoneNumber).VerifiedMobile(verifiedMobile).AllowEmailComms(allowEmailComms).AllowSmsComms(allowSmsComms).Build();
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            result.Should().BeTrue();
            candidate.RegistrationDetails.Should().NotBeNull();
            candidate.RegistrationDetails.PhoneNumber.Should().Be(phoneNumber);
            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.AllowEmail.Should().Be(allowEmailComms);
            candidate.CommunicationPreferences.AllowMobile.Should().Be(allowSmsComms);
            candidate.CommunicationPreferences.VerifiedMobile.Should().Be(verifiedMobile);
        }

        [TestCase("0123456789", false, false, false)]
        [TestCase("0123456789", true, false, false)]
        [TestCase("0123456789", false, true, true)]
        [TestCase("0123456789", true, true, false)]
        [TestCase("9876543210", false, false, false)]
        [TestCase("9876543210", true, false, false)]
        [TestCase("9876543210", false, true, true)]
        [TestCase("9876543210", true, true, true)]
        public void MobileVerificationRequired(string newPhoneNumber, bool verifiedMobile, bool allowSmsComms, bool mobileVerificationRequired)
        {
            const string mobileVerificationCode = "1234";
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            const string phoneNumber = "0123456789";
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).PhoneNumber(phoneNumber).Build);
            candidateService.Setup(cs => cs.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c =>
            {
                if (c.MobileVerificationRequired())
                    c.CommunicationPreferences.MobileVerificationCode = mobileVerificationCode;
                return c;
            });
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(newPhoneNumber).VerifiedMobile(verifiedMobile).AllowSmsComms(allowSmsComms).Build();
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            result.Should().BeTrue();
            candidate.RegistrationDetails.Should().NotBeNull();
            candidate.RegistrationDetails.PhoneNumber.Should().Be(newPhoneNumber);
            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.AllowMobile.Should().Be(allowSmsComms);

            if (newPhoneNumber != phoneNumber)
            {
                candidate.CommunicationPreferences.VerifiedMobile.Should().BeFalse();
            }
            else
            {
                candidate.CommunicationPreferences.VerifiedMobile.Should().Be(verifiedMobile);
            }

            if (mobileVerificationRequired)
            {
                candidate.MobileVerificationRequired().Should().BeTrue();
                candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
            }
            else
            {
                candidate.MobileVerificationRequired().Should().BeFalse();
                candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();
            }
        }
    }
}