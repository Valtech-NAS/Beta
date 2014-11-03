namespace SFA.Apprenticeships.Infrastructure.Address
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using NLog;

    public class AddressSearchProvider : IAddressSearchProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IMapper _mapper;

        public AddressSearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, IMapper mapper)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _mapper = mapper;
        }

        public IEnumerable<Address> FindAddress(string postcode)
        {
            Logger.Debug("FindAddress for postcode {0}", postcode);
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();
            
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof(Elastic.Common.Entities.Address));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.Address));

            var postcodeSearch = postcode.Replace(" ", "");

            var search = client.Search<Elastic.Common.Entities.Address>(s =>
            {
                s.Index(indexName);
                s.Type(documentTypeName);
                s.Size(100);
                s.Query(q => q.MatchPhrase(mpqd => mpqd.OnField(a => a.PostcodeSearch).Query(postcodeSearch)));
                return s;
            });

            //TODO: vga: refactor in a new ElasticClient class
            if (ThereWasAnErrorWhileSearching(search))
            {
                throw search.ConnectionStatus.OriginalException;
            }

            var addresses = _mapper
                .Map<IEnumerable<Elastic.Common.Entities.Address>, IEnumerable<Domain.Entities.Locations.Address>>(search.Documents)
                .ToList();

            SanitiseAddresses(addresses);

            return addresses;
        }

        private static void SanitiseAddresses(IEnumerable<Address> addresses)
        {
            foreach (var address in addresses)
            {
                address.AddressLine1 = SanitiseAddressLine(address.AddressLine1);
                address.AddressLine2 = SanitiseAddressLine(address.AddressLine2);
                address.AddressLine3 = SanitiseAddressLine(address.AddressLine3);
                address.AddressLine4 = SanitiseAddressLine(address.AddressLine4);
            }
        }

        private static string SanitiseAddressLine(string addressLine)
        {
            const string ampersand = "&";
            const string and = "and";

            return string.IsNullOrWhiteSpace(addressLine) ? addressLine 
                : addressLine.Replace(ampersand, and);
        }

        private static bool ThereWasAnErrorWhileSearching(Nest.ISearchResponse<Elastic.Common.Entities.Address> search)
        {
            return search != null &&
                            search.ConnectionStatus.Success &&
                            search.ConnectionStatus.OriginalException != null;
        }
    }
}
