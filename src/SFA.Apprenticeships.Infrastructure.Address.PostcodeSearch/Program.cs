using System;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Address.PostcodeSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var scroll = args[0];
            var size = int.Parse(args[1]);

            Console.WriteLine("Scroll: {0}, Size {1}", scroll, size);
            
            var clientFactory = new ElasticsearchClientFactory(ElasticsearchConfiguration.Instance, false);
            var client = clientFactory.GetElasticClient();

            Console.WriteLine("Connected. Press any key to start");
            Console.ReadKey();

            var indexName = clientFactory.GetIndexNameForType(typeof(Elastic.Common.Entities.Address));
            var documentTypeName = clientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.Address));
            var scanResults = client.Search<Elastic.Common.Entities.Address>(s => s
                .Index(indexName)
                .Type(documentTypeName)
                .From(0)
                .Size(size)
                .Filter(fd => fd.Not(fd2 => fd2.Exists(fd3 => fd3.PostcodeSearch)))
                .SearchType(SearchType.Scan)
                .Scroll(scroll)
            );

            var scrolls = 0;
            var totalUpdates = 0;
            var results = client.Scroll<Elastic.Common.Entities.Address>(GetScrollSelector(scanResults, scroll));
            while (results.Hits.Any())
            {
                var updates = 0;
                var descriptor = new BulkDescriptor();
                foreach (var address in results.Hits)
                {
                    AddPostcodeSearch(address, descriptor);
                    updates++;
                    totalUpdates++;
                }
                client.Bulk(descriptor);
                Console.WriteLine("Bulk updated {0} addresses. {1} addresses updated so far", updates, totalUpdates);

                results = client.Scroll<Elastic.Common.Entities.Address>(GetScrollSelector(results, scroll));
                scrolls++;
                Console.WriteLine("Updated batch {0}", scrolls);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void AddPostcodeSearch(IHit<Elastic.Common.Entities.Address> address, BulkDescriptor descriptor)
        {
            var id = address.Id;
            try
            {
                descriptor.Update<Elastic.Common.Entities.Address, object>(u => u
                            .Index("addresses")
                            .Id(id)
                            .Script("ctx._source.postcodesearch = ctx._source.postcode.replace(' ', '')")
                        );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Updating address id {0} failed! {1}", id, ex.StackTrace);
            }
        }

        private static ScrollDescriptor<Elastic.Common.Entities.Address> GetScrollSelector(ISearchResponse<Elastic.Common.Entities.Address> scanResults, string scroll)
        {
            return new ScrollDescriptor<Elastic.Common.Entities.Address>().Scroll(scroll).ScrollId(scanResults.ScrollId);
        }
    }
}
