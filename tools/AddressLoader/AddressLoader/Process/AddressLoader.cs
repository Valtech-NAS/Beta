using System;
using System.Linq;
using AddressLoader.Mongo;
using Nest;
using NLog;

namespace AddressLoader.Process
{
    public class AddressLoader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Uri _endpoint;
        private readonly string _mongoConnectionString;
        private readonly int _batchSize;
        private readonly string _aliasName;
        private readonly string _mongoDatabaseName;
        private readonly string _mongoCollectionName;

        public AddressLoader(string mongoConnectionString, string endpoint, int batchSize = 10000,
            string aliasName = "addresses", string mongoDatabaseName = "address_data", string mongoCollectionName = "addresses")
        {
            _endpoint = new Uri(endpoint);
            _batchSize = batchSize;
            _aliasName = aliasName;
            _mongoConnectionString = mongoConnectionString;
            _mongoDatabaseName = mongoDatabaseName;
            _mongoCollectionName = mongoCollectionName;
        }

        public void Run()
        {
            _logger.Debug("Connecting to {0}", _mongoConnectionString);
            var addressProvider = new MongoAddressProvider(_mongoConnectionString, _mongoDatabaseName, _mongoCollectionName);

            _logger.Debug("Connecting to {0}", _endpoint);
            var settings = new ConnectionSettings(_endpoint);

            var indexName = string.Format("{0}.{1:dd-MM-yyyy}", _aliasName, DateTime.Now);
            _logger.Debug("Using index \"{0}\"", indexName);

            settings.SetDefaultIndex(indexName);

            var client = new ElasticClient(settings);
            client.MapFromAttributes<AddressData>();

            _logger.Debug("Checking if indexed already exists");
            if (client.IndexExists(indexName).Exists)
            {
                _logger.Debug("Deleting existing index");
                client.DeleteIndex(indexName);
            }

            _logger.Debug("Creating new index");
            client.CreateIndex(indexName, i => i.AddMapping<AddressData>(m => m.MapFromAttributes()));

            _logger.Debug("Indexing \"{0}\" in batches of {1}...", indexName, _batchSize);
            var loop = 0;
            var total = 0;

            while (true)
            {
                var readStart = DateTime.Now;
                var batch = addressProvider.Fetch(loop*_batchSize, _batchSize).ToList();
                if (!batch.Any()) break;

                var indexStart = DateTime.Now;
                var result = client.IndexMany(batch, indexName);

                _logger.Debug("Read {0} records in {1}ms, indexed in {2}ms",
                    result.Items.Count(), 
                    indexStart.Subtract(readStart).TotalMilliseconds, 
                    DateTime.Now.Subtract(indexStart).TotalMilliseconds);

                loop++;
                total += batch.Count;

                //if (loop > 9) break; //todo: temp code for testing
            }

            _logger.Info("Indexed {0} records into \"{1}\" at {2}", total, indexName, _endpoint);

            _logger.Debug("Creating/swapping alias");

            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(_aliasName);
            client.Swap(_aliasName, existingIndexesOnAlias, new[] { indexName });

            _logger.Debug("Alias \"{0}\" now points to index \"{1}\"", _aliasName, indexName);
        }
    }
}
