namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.Mediators.Account;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;
    using Providers.AccountProvider;

    [TestFixture]
    public class CommunicationPreferencesTests
    {
        [TestCase("0123456789", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("0123456789", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("0123456789", true, true, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", true, false, AccountMediatorCodes.Settings.Success)]
        [TestCase("9876543210", false, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        [TestCase("9876543210", true, true, AccountMediatorCodes.Settings.MobileVerificationRequired)]
        public void MobileVerificationRequired(string newPhoneNumber, bool verifiedMobile, bool allowSmsComms, string expectedCode)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).PhoneNumber("0123456789").VerifiedMobile(true).Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var viewModel = new SettingsViewModelBuilder().PhoneNumber(newPhoneNumber).VerifiedMobile(verifiedMobile).AllowSmsComms(allowSmsComms).Build();
            var accountProvider = new AccountProviderBuilder().With(candidateService).Build();
            var mediator = new AccountMediatorBuilder().With(accountProvider).Build();

            var result = mediator.SaveSettings(candidateId, viewModel);

            if (expectedCode == AccountMediatorCodes.Settings.MobileVerificationRequired)
            {
                result.AssertMessage(expectedCode, AccountPageMessages.MobileVerificationRequired, UserMessageLevel.Success, true);
            }
            else
            {
                result.AssertCode(expectedCode);
            }
        }
    }
}