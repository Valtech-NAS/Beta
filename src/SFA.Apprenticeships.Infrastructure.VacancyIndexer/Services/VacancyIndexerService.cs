namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services
{
    using System;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;

    public class VacancyIndexerService : IVacancyIndexerService
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public VacancyIndexerService(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public void Index(VacancySummaryUpdate vacancySummaryToIndex)
        {
            var vacancySummaryElastic = _mapper.Map<VacancySummaryUpdate, VacancySummary>(vacancySummaryToIndex);
            var client = _elasticsearchClientFactory.GetElasticClient();
            client.Index(vacancySummaryElastic, _elasticsearchClientFactory.GetIndexNameForType(typeof(VacancySummary)));
        }

        public int VacanciesWithoutUpdateReference(Guid updateReference)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();

            var search = client.Count(new [] {"vacancies_search"}, new[] {"vacancy"}, qd =>
            {
                //qd.Text(t => t.)
                qd.QueryString(qs => qs.OnField("UpdateReference").Query(updateReference.ToString()));
                return qd;
            });

            return search.Count;
        }

        public void ClearObsoleteVacancie(Guid updateReference)
        {
            var client = _elasticsearchClientFactory.GetElasticClient();
            var index = _elasticsearchClientFactory.GetIndexNameForType(typeof (VacancySummary));
            
            client.DeleteByQuery(r =>
            {
                r.Index(index);
                r.Match(mqd => mqd.OnField("UpdateReference").QueryString(updateReference.ToString()));
                return r;
            });
        }
    }
}
