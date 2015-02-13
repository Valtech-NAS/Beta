
namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.VerifyMobileStrategy
{
    using Application.Candidate.Strategies;
    using Domain.Interfaces.Repositories;
    using Moq;

    public class VerifyMobileStrategyBuilder
    {
        private Mock<ICandidateReadRepository> _candidateReadRepositoryMock = new Mock<ICandidateReadRepository>();
        private Mock<ICandidateWriteRepository> _candidateWriteRepositoryMock = new Mock<ICandidateWriteRepository>();

        public VerifyMobileStrategyBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepositoryMock = candidateReadRepository;
            return this;
        }

        public VerifyMobileStrategyBuilder With(Mock<ICandidateWriteRepository> candidateWriteRepository)
        {
            _candidateWriteRepositoryMock = candidateWriteRepository;
            return this;
        }

        public VerifyMobileStrategy Build()
        {
            return new VerifyMobileStrategy(_candidateReadRepositoryMock.Object, _candidateWriteRepositoryMock.Object);
        }
    }
}
