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
                if (args == null || (args.Length != 2 && args.Length != 3))
                {
                    ShowHelp();
                    return;
                }
                #endregion

                var append = args.Length == 3; // any value in 3rd arg will be interpreted as "append"

                new Process.LocationLoader(args[0], args[1], append).Run();
            }

            catch (Exception ex)
            {
                Logger.Error("Failed" , ex);
            }
        }

        private static void ShowHelp()
        {
            Logger.Error("Invalid arguments\n\nExpected 2/3 arguments:\n  " +
                         "(1) relative path to CSV file containing location data\n  " + "" +
                         "(2) endpoint for ElasticSearch\n  " +
                         "(3) (optional) append to prevent truncation of existing data\n\n" +
                         "e.g. LocationLoader.exe \"my file.csv\" http://localhost:9200 append");
        }
    }
}
