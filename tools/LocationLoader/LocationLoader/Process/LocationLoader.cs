using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Nest;
using NLog;

namespace LocationLoader.Process
{
    public class LocationLoader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly bool _append;
        private readonly int _batchSize;
        private readonly string _indexName;
        private readonly string _filename;
        private readonly Uri _endpoint;

        public LocationLoader(string filename, string endpoint, bool append, int batchSize = 5000, string indexName = "locations")
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(string.Format("Cannot find location file \"{0}\"", filename), filename);

            _filename = filename;
            _endpoint = new Uri(endpoint);
            _append = append;
            _batchSize = batchSize;
            _indexName = indexName;
        }

        public void Run()
        {
            _logger.Info("Opening file {0}", _filename);

            using (TextReader reader = File.OpenText(_filename))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<LocationMapper>();

                _logger.Debug("Reading...");
                var allCsvRows = csv.GetRecords<LocationData>().ToList();

                _logger.Info("Read {0} records from file", allCsvRows.Count);

                _logger.Debug("Connecting to {0}", _endpoint);
                var settings = new ConnectionSettings(_endpoint);
                settings.SetDefaultIndex(_indexName);

                var client = new ElasticClient(settings);

                _logger.Debug("Checking if index already exists");
                if (client.IndexExists(_indexName).Exists)
                {
                    if (!_append)
                    {
                        _logger.Debug("Deleting existing index");
                        client.DeleteIndex(_indexName);
                    }
                }
                else
                {
                    if (_append)
                    {
                        throw new Exception("Cannot append to existing data because there is no existing index");
                    }                   
                }

                if (_append)
                {
                    _logger.Debug("Appending to existing index");
                }
                else
                {
                    _logger.Debug("Creating new index");

                    var indexSettings = new IndexSettings();
                    var keywordLowercaseCustomAnalyzer = new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase" } };
                    indexSettings.Analysis.Analyzers.Add("keywordlowercase", keywordLowercaseCustomAnalyzer);

                    client.CreateIndex(i => i.Index(_indexName).InitializeUsing(indexSettings));

                    client.Map<LocationData>(p => p.Index(_indexName).MapFromAttributes());
                }

                _logger.Debug("Indexing \"{0}\" in batches of {1}...", _indexName, _batchSize);
                var loop = 0;
                while (true)
                {
                    var batch = allCsvRows.Skip(loop*_batchSize).Take(_batchSize).ToList();
                    if (!batch.Any()) break;

                    var result = client.IndexMany(batch, _indexName);
                    _logger.Debug("Indexed {0} records in {1}ms", result.Items.Count(), result.Took);

                    loop++;
                }

                _logger.Info("Indexed {0} records into \"{1}\" at {2}", allCsvRows.Count, _indexName, _endpoint);
            }
        }
    }
}
