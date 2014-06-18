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
                if (args == null || args.Length != 2)
                {
                    ShowHelp();
                    return;
                }
                #endregion

                new Process.AddressLoader(args[0], args[1]).Run();
            }

            catch (Exception ex)
            {
                Logger.Error("Failed", ex);
            }
        }

        private static void ShowHelp()
        {
            Logger.Error("Invalid arguments\n\nExpected 2 arguments:\n  (1) connection string for MongoDB\n  (2) endpoint for ElasticSearch\n\ne.g. AddressLoader.exe mongodb://localhost:27017/ http://localhost:9200");
        }
    }
}
