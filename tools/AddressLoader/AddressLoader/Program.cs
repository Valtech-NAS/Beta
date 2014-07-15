using System;
using NLog;

namespace AddressLoader
{
    internal class Program
    {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                #region Guard
                if (args == null || (args.Length != 2 && args.Length != 3))
                {
                    ShowHelp();
                    return;
                }
                #endregion

                var isTestMode = args.Length == 3 && args[2].Equals("test", StringComparison.InvariantCultureIgnoreCase);

                new Process.AddressLoader(args[0], args[1]).Run(isTestMode);
            }

            catch (Exception ex)
            {
                Logger.Error("Failed", ex);
            }
        }

        private static void ShowHelp()
        {
            Logger.Error("Invalid arguments\n\nExpected 2 or 3 arguments:" +
                         "\n  (1) connection string for MongoDB" +
                         "\n  (2) endpoint for ElasticSearch" +
                         "\n  (3) optional TEST mode (fewer records processed)" +
                         "\n\ne.g. AddressLoader.exe mongodb://localhost:27017/ http://localhost:9200 TEST");
        }
    }
}
