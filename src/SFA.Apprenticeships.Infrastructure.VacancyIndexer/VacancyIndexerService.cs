namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer
{
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Services;

    public class VacancyIndexerService : IIndexerService<VacancySummaryUpdate>
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public void Index(VacancySummaryUpdate vacancySummaryUpdate)
        {
            var vacancySummaryElastic = _mapper.Map<VacancySummaryUpdate, VacancySummary>(vacancySummaryUpdate);
            var client = _elasticsearchClientFactory.GetElasticClient();
            client.Index(vacancySummaryElastic, _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary)));
        }
    }
}
