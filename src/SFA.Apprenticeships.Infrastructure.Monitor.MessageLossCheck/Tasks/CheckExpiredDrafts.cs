namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System.Linq;
    using Elastic.Common.Configuration;
    using Elastic.Common.Entities;
    using Monitor.Tasks;
    using NLog;
    using Repository;

    public class CheckExpiredDrafts : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApprenticeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public CheckExpiredDrafts(IApprenticeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _elasticsearchClientFactory = elasticsearchClientFactory;
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
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary));

            Logger.Info("Getting vacancy ids in index");

            var searchResponse = client.Search<ApprenticeshipSummary>(s => s
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
        }
    }
}