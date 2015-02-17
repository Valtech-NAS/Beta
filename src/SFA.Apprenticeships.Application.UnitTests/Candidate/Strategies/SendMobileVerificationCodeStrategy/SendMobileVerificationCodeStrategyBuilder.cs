namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SendMobileVerificationCodeStrategy
{
    using Application.Candidate.Strategies;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;

    public class SendMobileVerificationCodeStrategyBuilder
    {
        private Mock<ICommunicationService> _communicationService;
        private Mock<ICandidateWriteRepository> _candidateWriteRepository;
        private Mock<ICodeGenerator> _codeGenerator;

        public SendMobileVerificationCodeStrategyBuilder()
        {
            _communicationService = new Mock<ICommunicationService>();
            _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            _codeGenerator = new Mock<ICodeGenerator>();
        }

        public SendMobileVerificationCodeStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }

        public SendMobileVerificationCodeStrategyBuilder With(Mock<ICandidateWriteRepository> candidateWriteRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            return this;
        }

        public SendMobileVerificationCodeStrategyBuilder With(Mock<ICodeGenerator> codeGenerator)
        {
            _codeGenerator = codeGenerator;
            return this;
        }

        public SendMobileVerificationCodeStrategy Build()
        {
            var strategy = new SendMobileVerificationCodeStrategy(_communicationService.Object, _candidateWriteRepository.Object, _codeGenerator.Object);
            return strategy;
        }
    }
}