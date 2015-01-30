namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Elastic.Common.Configuration;
    using Monitor.Tasks;
    using NLog;
    using Repository;

    public class CheckExpiredDrafts : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApprenticeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _vacancySearchService;

        public CheckExpiredDrafts(IApprenticeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IElasticsearchClientFactory elasticsearchClientFactory, IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> vacancySearchService)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _vacancySearchService = vacancySearchService;
        }

        public string TaskName
        {
            get { return "Check Expired Drafts"; }
        }

        public void Run()
        {
            Logger.Info("Getting draft vacancy ids");

            var draftVacancyIds = _applicationDiagnosticsRepository.GetDraftApplicationVacancyIds().ToList();

            var draftVacancyIdsString = string.Join(", ", draftVacancyIds);
            Logger.Info("Draft vacancy ids {0}: {1}", draftVacancyIds.Count, draftVacancyIdsString);

            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(Elastic.Common.Entities.ApprenticeshipSummary));

            Logger.Info("Getting vacancy ids in index");

            var searchResponse = client.Search<Elastic.Common.Entities.ApprenticeshipSummary>(s => s
                .Index(indexName)
                .Fields("id")
                .Filter(fc => fc.Ids(draftVacancyIds))
                .Size(draftVacancyIds.Count));

            var vacancyIdsInIndex = searchResponse.Hits.Select(v => v.Id).ToList();

            var vacancyIdsInIndexString = string.Join(", ", vacancyIdsInIndex);
            Logger.Info("Vacancy ids in index {0}: {1}", vacancyIdsInIndex.Count, vacancyIdsInIndexString);

            var expiredVacancyIds = draftVacancyIds.Except(vacancyIdsInIndex).ToList();

            var expiredVacancyIdsString = string.Join(", ", expiredVacancyIds);
            Logger.Info("Expired vacancy ids {0}: {1}", expiredVacancyIds.Count, expiredVacancyIdsString);

            /*var verifiedExpiredVacancyIds = expiredVacancyIds.ToList();

            Logger.Info("Verifying expired vacancies");
            foreach (var expiredVacancyId in expiredVacancyIds)
            {
                int vacancyId;
                if (int.TryParse(expiredVacancyId, out vacancyId))
                {
                    var vacancyDetails = _vacancySearchService.GetVacancyDetails(vacancyId);
                    if (vacancyDetails != null)
                    {
                        Logger.Info("Vacancy with id: {0} has not expired and will be removed", vacancyId);
                        verifiedExpiredVacancyIds.Remove(expiredVacancyId);
                    }
                }
                else
                {
                    Logger.Info("Vacancy id: {0} could not be parsed so assume still valid", expiredVacancyId);
                    verifiedExpiredVacancyIds.Remove(expiredVacancyId);
                }
            }

            var verifiedExpiredVacancyIdsString = string.Join(", ", verifiedExpiredVacancyIds);
            Logger.Info("Verified expired vacancy ids {0}: {1}", verifiedExpiredVacancyIds.Count, verifiedExpiredVacancyIdsString);*/
        }
    }
}