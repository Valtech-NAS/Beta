namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveCandidateStrategyTests
    {
        private readonly Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository =
            new Mock<IApprenticeshipApplicationReadRepository>();
        private readonly Mock<IApprenticeshipApplicationWriteRepository> _apprenticeshipApplicationWriteRepository =
            new Mock<IApprenticeshipApplicationWriteRepository>();
        private readonly Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private readonly Mock<ICandidateWriteRepository> _candidateWriteRepository =
            new Mock<ICandidateWriteRepository>();
        private readonly Mock<IGetCandidateApprenticeshipApplicationsStrategy> _getCandidateApplicationsStrategy =
            new Mock<IGetCandidateApprenticeshipApplicationsStrategy>();
        
        [Test]
        public void ShouldUpdateDraftApprenticeshipApplicationDetails()
        {
            var newRegistrationDetails = new RegistrationDetails
            {
                EmailAddress = "updatedEmailAddress@gmail.com"
            };

            _candidateReadRepository.Setup(crr => crr.Get(It.IsAny<Guid>())).Returns(new Candidate{RegistrationDetails = newRegistrationDetails});
            _getCandidateApplicationsStrategy.Setup(gca => gca.GetApplications(It.IsAny<Guid>()))
                .Returns(new[] {new ApprenticeshipApplicationSummary {Status = ApplicationStatuses.Draft}});
            _apprenticeshipApplicationReadRepository.Setup(
                aprr => aprr.GetForCandidate(It.IsAny<Guid>(), It.IsAny<int>(),false))
                .Returns(new ApprenticeshipApplicationDetail());

            var saveCandidateStrategy = new SaveCandidateStrategy(_candidateWriteRepository.Object,
                _getCandidateApplicationsStrategy.Object, _candidateReadRepository.Object,
                _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, null, null, null);

            saveCandidateStrategy.SaveCandidate(new Candidate());

            _apprenticeshipApplicationWriteRepository.Verify(
                aawr =>
                    aawr.Save(It.Is<ApprenticeshipApplicationDetail>(a => a.CandidateDetails.EmailAddress == newRegistrationDetails.EmailAddress)));

        }

        [Test]
        public void ShouldNotUpdateNoDraftApprenticeshipApplicationDetails()
        {
            var newRegistrationDetails = new RegistrationDetails
            {
                EmailAddress = "updatedEmailAddress@gmail.com"
            };

            _candidateReadRepository.Setup(crr => crr.Get(It.IsAny<Guid>())).Returns(new Candidate { RegistrationDetails = newRegistrationDetails });
            _getCandidateApplicationsStrategy.Setup(gca => gca.GetApplications(It.IsAny<Guid>()))
                .Returns(new[]
                {
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Successful },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.ExpiredOrWithdrawn },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.InProgress },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Submitted },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Submitting },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Unknown },
                    new ApprenticeshipApplicationSummary { Status = ApplicationStatuses.Unsuccessful }
                });
            _apprenticeshipApplicationReadRepository.Setup(
                aprr => aprr.GetForCandidate(It.IsAny<Guid>(), It.IsAny<int>(), false))
                .Returns(new ApprenticeshipApplicationDetail());

            var saveCandidateStrategy = new SaveCandidateStrategy(_candidateWriteRepository.Object,
                _getCandidateApplicationsStrategy.Object, _candidateReadRepository.Object,
                _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, null, null, null);

            saveCandidateStrategy.SaveCandidate(new Candidate());

            _apprenticeshipApplicationWriteRepository.Verify(
                aawr =>
                    aawr.Save(It.Is<ApprenticeshipApplicationDetail>(a => a.CandidateDetails.EmailAddress == newRegistrationDetails.EmailAddress)), Times.Never());

        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldAssignAndSendMobileUserCodeIfVerificationIsRequired(bool verifiedMobile)
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";
            var candidate = new Candidate
            {
                EntityId = candidateId,
                RegistrationDetails = new RegistrationDetails
                {
                    PhoneNumber = phoneNumber
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    AllowMobile = true,
                    VerifiedMobile = verifiedMobile
                }
            };

            var codeGenerator = new Mock<ICodeGenerator>();
            const string mobileVerificationCode = "1234";
            codeGenerator.Setup(cg => cg.GenerateNumeric(4)).Returns(mobileVerificationCode);
            _getCandidateApplicationsStrategy.Setup(gca => gca.GetApplications(candidateId)).Returns(new ApprenticeshipApplicationSummary[0]);
            var communicationService = new Mock<ICommunicationService>();
            IEnumerable<CommunicationToken> communicationTokens = new List<CommunicationToken>(0);
            communicationService.Setup(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>())).Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((cid, mt, ct) => { communicationTokens = ct; });

            var sendMobileVerificationCodeStrategy = new SendMobileVerificationCodeStrategy(communicationService.Object);
            var saveCandidateStrategy = new SaveCandidateStrategy(_candidateWriteRepository.Object, _getCandidateApplicationsStrategy.Object, _candidateReadRepository.Object, _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, codeGenerator.Object, sendMobileVerificationCodeStrategy, null);

            saveCandidateStrategy.SaveCandidate(candidate);

            if (verifiedMobile)
            {
                candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();
                communicationService.Verify(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
            }
            else
            {
                candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
                communicationService.Verify(cs => cs.SendMessageToCandidate(candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
                var communicationTokensList = communicationTokens.ToList();
                communicationTokensList.Count.Should().Be(2);
                communicationTokensList.Single(ct => ct.Key == CommunicationTokens.CandidateMobileNumber).Value.Should().Be(phoneNumber);
                communicationTokensList.Single(ct => ct.Key == CommunicationTokens.MobileVerificationCode).Value.Should().Be(mobileVerificationCode);
            }
        }
    }
}