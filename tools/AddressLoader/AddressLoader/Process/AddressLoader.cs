namespace AddressLoader.Process
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Mongo;
    using Nest;
    using NLog;

    public class AddressLoader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _aliasName;
        private readonly int _batchSize;
        private readonly Uri _endpoint;
        private readonly string _mongoCollectionName;
        private readonly string _mongoConnectionString;
        private readonly string _mongoDatabaseName;

        public AddressLoader(string mongoConnectionString, string endpoint, int batchSize = 10000,
            string aliasName = "addresses", string mongoDatabaseName = "address_data",
            string mongoCollectionName = "addresses")
        {
            _endpoint = new Uri(endpoint);
            _batchSize = batchSize;
            _aliasName = aliasName;
            _mongoConnectionString = mongoConnectionString;
            _mongoDatabaseName = mongoDatabaseName;
            _mongoCollectionName = mongoCollectionName;
        }

        public void Run(bool isTestMode)
        {
            if (isTestMode) _logger.Debug("** TEST MODE **");

            _logger.Debug("Connecting to {0}", _mongoConnectionString);
            var addressProvider = new MongoAddressProvider(_mongoConnectionString, _mongoDatabaseName,
                _mongoCollectionName);

            _logger.Debug("Connecting to {0}", _endpoint);
            var settings = new ConnectionSettings(_endpoint);

            var indexName = string.Format("{0}.{1:yyyy-MM-dd}", _aliasName, DateTime.Now);
            _logger.Debug("Using index \"{0}\"", indexName);

            settings.SetDefaultIndex(indexName);

            var client = new ElasticClient(settings);
            client.MapFromAttributes<AddressData>();

            _logger.Debug("Checking if index already exists");
            if (client.IndexExists(indexName).Exists)
            {
                _logger.Debug("Deleting existing index");
                client.DeleteIndex(indexName);
            }

            _logger.Debug("Creating new index");
            client.CreateIndex(indexName, i => i.AddMapping<AddressData>(m => m.MapFromAttributes()));

            _logger.Debug("Indexing \"{0}\" in batches of {1}...", indexName, _batchSize);
            var total = 0;
            var batchItemCount = 0;
            var batchItems = new List<AddressData>();
            var readWatch = new Stopwatch();
            readWatch.Start();

            foreach (var address in addressProvider.AllResidentialAddresses)
            {
                if (batchItemCount == _batchSize)
                {
                    readWatch.Stop();
                    batchItemCount = 0;
                    total += _batchSize;

                    IndexItems(client, batchItems, indexName, readWatch.ElapsedMilliseconds);

                    batchItems.Clear();
                    readWatch.Reset();
                    readWatch.Start();
                }

                var add = address.ToAddress();
                batchItems.Add(add);
                batchItemCount++;

                if (isTestMode && total >= _batchSize) break;
            }

            if (!isTestMode)
            {
                IndexItems(client, batchItems, indexName, readWatch.ElapsedMilliseconds);
                total += batchItemCount;
            }

            _logger.Info("Indexed {0} records into \"{1}\" at {2}", total, indexName, _endpoint);

            _logger.Debug("Creating/swapping alias");

            var existingIndexesOnAlias = client.GetIndicesPointingToAlias(_aliasName);
            client.Swap(_aliasName, existingIndexesOnAlias, new[] {indexName});

            _logger.Debug("Alias \"{0}\" now points to index \"{1}\"", _aliasName, indexName);
        }


        private void IndexItems(ElasticClient client, IEnumerable<AddressData> batchItems, string indexName, long readMilliseconds)
        {
            var indexWatch = new Stopwatch();
            indexWatch.Start();
            var result = client.IndexMany(batchItems, indexName);
            indexWatch.Stop();

            _logger.Debug("Read {0} records in {1}ms, indexed in {2}ms",
                result.Items.Count(),
                readMilliseconds,
                indexWatch.ElapsedMilliseconds);
        }
    }
}
