namespace SFA.Apprenticeships.Service.Vacancy.TestLoader
{
    using System;
    using System.IO;
    using System.Linq;
    using Application.VacancyEtl.Entities;
    using CsvHelper;
    using CsvLoader;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.VacancyIndexer;
    using Infrastructure.VacancyIndexer.IoC;
    using NLog;
    using StructureMap;

    public class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            if (!CheckArgs(args))
            {
                return;
            }

            _logger.Debug("Loading IoC configuration...");

            ObjectFactory.Configure(c =>
            {
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
                c.AddRegistry<VacancyIndexerRegistry>();
            });

            _logger.Debug("Loaded IoC configuration...");

            _logger.Debug("Loading Csv file: {0}", args[0]);

            var indexer = ObjectFactory.GetInstance<IVacancyIndexerService>();
            var indexDate = DateTime.Today;

            using (TextReader reader = File.OpenText(args[0]))
            {
                _logger.Debug("Loaded Csv file: {0}", args[0]);

                _logger.Debug("Loading Csv Mapping");
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<VacancyMapper>();
                _logger.Debug("Loaded Csv Mapping");

                _logger.Debug("Loading Csv Rows");
                var allCsvRows = csv.GetRecords<VacancySummaryUpdate>().ToList();
                _logger.Debug("Loaded '{0}' Csv Rows", allCsvRows.Count);

                _logger.Debug("Creating index for date: ", indexDate);
                indexer.CreateScheduledIndex(indexDate);
                _logger.Debug("Created index for date: ", indexDate);

                foreach (VacancySummaryUpdate vacancySummaryUpdate in allCsvRows)
                {
                    _logger.Debug("Indexing item: {0}", vacancySummaryUpdate.Title);
                    vacancySummaryUpdate.ScheduledRefreshDateTime = indexDate;
                    indexer.Index(vacancySummaryUpdate);
                    _logger.Debug("Indexed item: {0}", vacancySummaryUpdate.Title);
                }

                _logger.Debug("Swapping index");
                indexer.SwapIndex(indexDate);
                _logger.Debug("Swapped index");
            }
            
            _logger.Debug("Complete");
        }

        private static bool CheckArgs(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("The usage requires the following paramters:");
                Console.WriteLine("SFA.Apprenticeships.Service.Vacancy.TestLoader.exe <PathToCsvData>");
                return false;
            }

            return true;
        }
    }
}
