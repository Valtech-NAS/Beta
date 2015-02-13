namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.VerifyMobileStrategy
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyMobileStrategyTests
    {

        [Test]
        public void SuccessTest()
        {
            //Arrange
            Guid candidateId = Guid.NewGuid();
            string verificationCode = "1234";

            Candidate candidate = new CandidateBuilder(candidateId).MobileVerificationCode(verificationCode).AllowMobile(true).VerifiedMobile(false).Build();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(candidate);

            //candidateReadRepository.Setup()
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            
            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });

            var verifyMobileStrategy = new VerifyMobileStrategyBuilder().With(candidateReadRepository).With(candidateWriteRepository).Build();

            //Act
            verifyMobileStrategy.VerifyMobile(candidateId, verificationCode);

            //Assert
            candidateWriteRepository.Verify(r => r.Save(It.IsAny<Candidate>()), Times.Once);
            candidate.CommunicationPreferences.AllowMobile.Should().BeTrue();
            candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();
            candidate.CommunicationPreferences.VerifiedMobile.Should().BeTrue();
            candidate.MobileVerificationRequired().Should().BeFalse();
        }

        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ErrorTest(bool allowMobile, bool verifiedMobile)
        {
            //Arrange
            Guid candidateId = Guid.NewGuid();
            string verificationCode = string.Empty;

            Candidate candidate = new CandidateBuilder(candidateId).MobileVerificationCode(verificationCode).AllowMobile(allowMobile).VerifiedMobile(verifiedMobile).Build();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(candidate);

            
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();

            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });

            var verifyMobileStrategy = new VerifyMobileStrategyBuilder().With(candidateReadRepository).With(candidateWriteRepository).Build();

            //Act
            Action a = () => verifyMobileStrategy.VerifyMobile(candidateId, verificationCode);

            //Assert
            var errorMessage = string.Format("The mobile number associated with candidate Id: {0} does not require verification.", candidateId);
            a.ShouldThrow<CustomException>().WithMessage(errorMessage);
            a.ShouldThrow<CustomException>();
        }

        [Test]
        public void MobileCodeVerificationFailedTest()
        {
            //Arrange
            Guid candidateId = Guid.NewGuid();
            string actualVerificationCode = "1234";
            string enteredVerificationCode = "5678";

            Candidate candidate = new CandidateBuilder(candidateId).MobileVerificationCode(actualVerificationCode).AllowMobile(true).VerifiedMobile(false).Build();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            candidateReadRepository.Setup(r => r.Get(candidateId)).Returns(candidate);


            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();

            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Callback<Candidate>(c => { candidate = c; });

            var verifyMobileStrategy = new VerifyMobileStrategyBuilder().With(candidateReadRepository).With(candidateWriteRepository).Build();

            //Act
            Action a = () => verifyMobileStrategy.VerifyMobile(candidateId, enteredVerificationCode);

            //Assert
            var errorMessage = string.Format("Mobile verification code {0} is invalid for candidate {1} with mobile number {2}", enteredVerificationCode, 
                                                                        candidateId, candidate.RegistrationDetails.PhoneNumber);
            a.ShouldThrow<CustomException>().WithMessage(errorMessage);
        }
    }
}