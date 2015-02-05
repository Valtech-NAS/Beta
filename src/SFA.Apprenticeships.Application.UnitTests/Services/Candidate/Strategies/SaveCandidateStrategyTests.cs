namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies
{
    using System;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
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
                _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, null);

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
                _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, null);

            saveCandidateStrategy.SaveCandidate(new Candidate());

            _apprenticeshipApplicationWriteRepository.Verify(
                aawr =>
                    aawr.Save(It.Is<ApprenticeshipApplicationDetail>(a => a.CandidateDetails.EmailAddress == newRegistrationDetails.EmailAddress)), Times.Never());

        }
    }
}