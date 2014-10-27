using System.Runtime.InteropServices;
using NLog;

namespace SampleDataManufacturer
{   
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var numUsers = int.Parse(args[0]);
            var numApplications = int.Parse(args[1]);

            Logger.Info("Creating {0} users and {1} applications for each user.", numUsers, numApplications);

            var dataManufacturer = new DataManufacturer();

            for (var i = 0; i < numUsers; i++)
            {
                Logger.Info("Creating user {0}", i);
                var username = dataManufacturer.CreateUser();

                for (var j = 0; j < numApplications; j++)
                {
                    Logger.Info("Creating application {0} for user {1}.", j, i);
                    dataManufacturer.CreateApplication(username, j);
                }
            }
        }
    }
}
