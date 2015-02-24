namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Interfaces.Mapping;
    using Moq;

    public class TraineeshipApplicationProviderBuilder
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private Mock<ICandidateService> _candidateService;
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider;

        public TraineeshipApplicationProviderBuilder()
        {
            _mapper = new TraineeshipCandidateWebMappers();
            _logger = new Mock<ILogService>().Object;

            _candidateService = new Mock<ICandidateService>();
            _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
        }

        public TraineeshipApplicationProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public TraineeshipApplicationProviderBuilder With(Mock<ITraineeshipVacancyDetailProvider> traineeshipVacancyDetailProvider)
        {
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
            return this;
        }

        public TraineeshipApplicationProvider Build()
        {
            var provider = new TraineeshipApplicationProvider(_mapper, _candidateService.Object, _traineeshipVacancyDetailProvider.Object, _logger);
            return provider;
        }
    }
}