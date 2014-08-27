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

            ObjectFactory.Configure(c =>
            {
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
                c.AddRegistry<VacancyIndexerRegistry>();
            });

            using (TextReader reader = File.OpenText(args[0]))
            {
                var csv = new CsvReader(reader);
                csv.Configuration.RegisterClassMap<VacancyMapper>();
                
                _logger.Debug("Reading...");
                var allCsvRows = csv.GetRecords<VacancySummaryUpdate>().ToList();

                var indexer = ObjectFactory.GetInstance<IVacancyIndexerService>();

                var indexDate = DateTime.Today;

                indexer.CreateScheduledIndex(indexDate);

                foreach (VacancySummaryUpdate vacancySummaryUpdate in allCsvRows)
                {
                    vacancySummaryUpdate.ScheduledRefreshDateTime = indexDate;
                    indexer.Index(vacancySummaryUpdate);    
                }
                
                indexer.SwapIndex(indexDate);
            }
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
