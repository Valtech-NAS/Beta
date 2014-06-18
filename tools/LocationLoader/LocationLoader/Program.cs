using System;
using LocationLoader;
using NLog;

namespace LocationLoader
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

                new Process.LocationLoader(args[0], args[1]).Run();
            }

            catch (Exception ex)
            {
                Logger.Error("Failed" , ex);
            }
        }

        private static void ShowHelp()
        {
            Logger.Error("Invalid arguments\n\nExpected 2 arguments:\n  (1) relative path to CSV file containing location data\n  (2) endpoint for ElasticSearch\n\ne.g. LocationLoader.exe \"my file.csv\" http://localhost:9200");
        }
    }
}
