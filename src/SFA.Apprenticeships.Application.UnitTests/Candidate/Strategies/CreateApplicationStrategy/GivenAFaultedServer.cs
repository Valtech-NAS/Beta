namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.CreateApplicationStrategy
{
    using System;
    using Application.Candidate.Strategies.Apprenticeships;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using Vacancy;

    [TestFixture]
    public class GivenAFaultedServer
    {
        [TestCase, ExpectedException(typeof(Exception))]
        public void WhenCreatingANewApplication_ShouldThrowACustomException()
        {
            var vacancyDataProvider = new Mock<IVacancyDataProvider<ApprenticeshipVacancyDetail>>();
            var applicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            var applicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();

            applicationReadRepository.Setup(arr => arr.GetForCandidate(It.IsAny<Guid>(),
                It.IsAny<int>(), It.IsAny<bool>())).Throws<Exception>();

            var createApplicationStrategy = new CreateApprenticeshipApplicationStrategy(vacancyDataProvider.Object,
                applicationReadRepository.Object, applicationWriteRepository.Object,
                candidateReadRepository.Object);

            createApplicationStrategy.CreateApplication(Guid.NewGuid(), 1);
        }
    }
}