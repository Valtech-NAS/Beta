namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Locations;
    using CuttingEdge.Conditions;
    using Elastic.Common.Configuration;
    using Nest;

    public class AddressSearchProvider : IAddressSearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public AddressSearchProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public IEnumerable<Domain.Entities.Locations.Address> FindAddresses(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(Elastic.Common.Entities.Address));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.Address));

            var search = client.Search<Domain.Entities.Locations.Address>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.Size(100);
                s.Query(q => q.MatchPhrase(mpqd => mpqd.OnField(a => a.Postcode).QueryString(postcode)));
                return s;
            });

            return search.Documents;
        }
    }
}
