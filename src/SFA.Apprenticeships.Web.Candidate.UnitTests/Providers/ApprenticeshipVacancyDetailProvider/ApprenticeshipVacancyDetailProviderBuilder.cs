namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApprenticeshipVacancyDetailProvider
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Vacancies;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Moq;

    public class ApprenticeshipVacancyDetailProviderBuilder
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> _vacancySearchService;
        private Mock<ICandidateService> _candidateService;

        public ApprenticeshipVacancyDetailProviderBuilder()
        {
            _mapper = new ApprenticeshipCandidateWebMappers();
            _logger = new Mock<ILogService>().Object;

            _vacancySearchService = new Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            _candidateService = new Mock<ICandidateService>();
        }

        public ApprenticeshipVacancyDetailProviderBuilder With(Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> vacancySearchService)
        {
            _vacancySearchService = vacancySearchService;
            return this;
        }

        public ApprenticeshipVacancyDetailProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public ApprenticeshipVacancyDetailProvider Build()
        {
            var provider = new ApprenticeshipVacancyDetailProvider(_vacancySearchService.Object, _candidateService.Object, _mapper, _logger);
            return provider;
        }
    }
}