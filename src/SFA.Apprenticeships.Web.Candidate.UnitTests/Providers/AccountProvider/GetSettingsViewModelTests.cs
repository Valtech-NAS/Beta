namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetSettingsViewModelTests
    {
        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void TestCommunicationMappings(bool verifiedMobile, bool allowEmailComms, bool allowSmsComms)
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";
            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(phoneNumber)
                .AllowEmail(allowEmailComms)
                .AllowMobile(allowSmsComms)
                .VerifiedMobile(verifiedMobile)
                .Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();
            viewModel.PhoneNumber.Should().Be(phoneNumber);
            viewModel.VerifiedMobile.Should().Be(verifiedMobile);
            viewModel.AllowEmailComms.Should().Be(allowEmailComms);
            viewModel.AllowSmsComms.Should().Be(allowSmsComms);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void TestMarketingMappings(bool allowEmailMarketing, bool allowSmsMarketing)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId)
                .AllowEmailMarketing(allowEmailMarketing)
                .AllowMobileMarketing(allowSmsMarketing)
                .Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();
            viewModel.AllowEmailMarketing.Should().Be(allowEmailMarketing);
            viewModel.AllowSmsMarketing.Should().Be(allowSmsMarketing);
        }
    }
}