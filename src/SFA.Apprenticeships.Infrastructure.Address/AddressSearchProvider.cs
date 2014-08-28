namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Locations;
    using CuttingEdge.Conditions;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;

    public class AddressSearchProvider : IAddressSearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public AddressSearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public IEnumerable<Domain.Entities.Locations.Address> FindAddress(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(Elastic.Common.Entities.Address));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.Address));

            var search = client.Search<Elastic.Common.Entities.Address>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.Size(100);
                s.Query(q => q.MatchPhrase(mpqd => mpqd.OnField(a => a.Postcode).Query(postcode)));
                return s;
            });

            var addresses =
                _mapper
                    .Map<IEnumerable<Elastic.Common.Entities.Address>, IEnumerable<Domain.Entities.Locations.Address>>(
                        search.Documents);

            return  addresses;
        }
    }
}
