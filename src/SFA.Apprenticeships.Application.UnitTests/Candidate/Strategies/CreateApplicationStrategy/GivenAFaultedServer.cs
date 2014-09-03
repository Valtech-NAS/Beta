namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.CreateApplicationStrategy
{
    using System;
    using Application.Candidate.Strategies;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using Vacancy;

    [TestFixture]
    public class GivenAFaultedServer
    {
        [TestCase, ExpectedException(typeof(CustomException))]
        public void WhenCreateingANewApplication_ShouldThrowACustomException()
        {
            var vacancyDataProvider = new Mock<IVacancyDataProvider>();
            var applicationReadRepository = new Mock<IApplicationReadRepository>();
            var applicationWriteRepository = new Mock<IApplicationWriteRepository>();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();

            applicationReadRepository.Setup(arr => arr.GetForCandidate(It.IsAny<Guid>(),
                It.IsAny<Func<ApplicationDetail, bool>>())).Throws<Exception>();

            var createApplicationStrategy = new CreateApplicationStrategy(vacancyDataProvider.Object,
                applicationReadRepository.Object, applicationWriteRepository.Object,
                candidateReadRepository.Object);

            createApplicationStrategy.CreateApplication(Guid.NewGuid(), 1);
        }
    }
}