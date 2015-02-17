namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SendMobileVerificationCodeStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    [TestFixture]
    public class SendMobileVerificationCodeStrategyTests
    {
        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void MobileVerificationNotRequired(bool allowMobile, bool verifiedMobile)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).AllowMobile(allowMobile).VerifiedMobile(verifiedMobile).Build();
            var strategy = new SendMobileVerificationCodeStrategyBuilder().Build();

            Action action = () => { strategy.SendMobileVerificationCode(candidate); };

            var message = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidateId);
            action.ShouldThrow<CustomException>().WithMessage(message).And.Code.Should().Be(ErrorCodes.EntityStateError);
        }

        [Test]
        public void EmptyMobileVerificationCode()
        {
            var candidateId = Guid.NewGuid();
            const string mobileVerificationCode = "1234";
            var candidate = new CandidateBuilder(candidateId).AllowMobile(true).VerifiedMobile(false).MobileVerificationCode(string.Empty).Build();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });
            var codeGenerator = new Mock<ICodeGenerator>();
            codeGenerator.Setup(cg => cg.GenerateNumeric(4)).Returns(mobileVerificationCode);
            var strategy = new SendMobileVerificationCodeStrategyBuilder().With(candidateWriteRepository).With(codeGenerator).Build();

            strategy.SendMobileVerificationCode(candidate);

            candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
            candidateWriteRepository.Verify(r => r.Save(candidate), Times.Once);
        }

        [Test]
        public void Success()
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";
            const string mobileVerificationCode = "1234";
            var candidate = new CandidateBuilder(candidateId).AllowMobile(true).VerifiedMobile(false).MobileVerificationCode(mobileVerificationCode).PhoneNumber(phoneNumber).Build();
            var communicationService = new Mock<ICommunicationService>();
            IEnumerable<CommunicationToken> communicationTokens = new List<CommunicationToken>(0);
            communicationService.Setup(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>())).Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((cid, mt, ct) => { communicationTokens = ct; });
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var codeGenerator = new Mock<ICodeGenerator>();
            var strategy = new SendMobileVerificationCodeStrategyBuilder().With(communicationService).With(candidateWriteRepository).With(codeGenerator).Build();

            strategy.SendMobileVerificationCode(candidate);

            candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
            candidateWriteRepository.Verify(r => r.Save(candidate), Times.Never);
            candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
            communicationService.Verify(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
            var communicationTokensList = communicationTokens.ToList();
            communicationTokensList.Count.Should().Be(2);
            communicationTokensList.Single(ct => ct.Key == CommunicationTokens.CandidateMobileNumber).Value.Should().Be(phoneNumber);
            communicationTokensList.Single(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value.Should().Be(mobileVerificationCode);
            codeGenerator.Verify(cg => cg.GenerateNumeric(It.IsAny<int>()), Times.Never);
        }
    }
}