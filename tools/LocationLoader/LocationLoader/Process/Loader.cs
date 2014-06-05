using System;
using System.IO;
using System.Linq;
using CsvHelper;
using Nest;
using NLog;

namespace LocationLoader.Process
{
    public class Loader
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const int BatchSize = 5000;
        private const string IndexName = "locations";
        private readonly string _filename;
        private readonly Uri _endpoint;

        public Loader(string filename, string endpoint)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(string.Format("Cannot find location file \"{0}\"", filename), filename);

            _filename = filename;
            _endpoint = new Uri(endpoint);
        }

        public void Run()
        {
            _logger.Info("Opening file {0}", _filename);

            using (TextReader reader = File.OpenText(_filename))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<Mapper>();

                _logger.Debug("Reading...");
                var allCsvRows = csv.GetRecords<Location>().ToList();

                _logger.Info("Read {0} records from file", allCsvRows.Count);

                _logger.Debug("Connecting to {0}", _endpoint);
                var settings = new ConnectionSettings(_endpoint);
                settings.SetDefaultIndex("locations");

                var client = new ElasticClient(settings);
                client.MapFromAttributes<Location>(IndexName);

                _logger.Debug("Checking if index already exists");
                if (client.IndexExists(IndexName).Exists)
                {
                    _logger.Debug("Deleting existing index");
                    client.DeleteIndex(IndexName);
                }

                _logger.Debug("Creating index");
                client.CreateIndex(IndexName, i => i.AddMapping<Location>(m => m.MapFromAttributes()));

                _logger.Debug("Indexing \"{0}\" in batches of {1}...", IndexName, BatchSize);
                var loop = 0;
                while (true)
                {
                    var batch = allCsvRows.Skip(loop*BatchSize).Take(BatchSize).ToList();
                    if (!batch.Any()) break;

                    var result = client.IndexMany(batch, IndexName);
                    _logger.Debug("Indexed {0} records in {1}ms", result.Items.Count(), result.Took);

                    loop++;
                }

                _logger.Info("Indexed {0} records into \"{1}\" at {2}", allCsvRows.Count, IndexName, _endpoint);
            }
        }
    }
}
