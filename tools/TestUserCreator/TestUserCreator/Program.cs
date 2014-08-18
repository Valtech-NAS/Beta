namespace TestUserCreator
{
    using System;
    using System.Collections.Generic;
    using NLog;
    using Process;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            try
            {
                int count;
                var config = ParseArgs(args, out count);

                if (config == null)
                {
                    ShowUsage();
                    return;
                }

                new CreateTestUsersProcess(config).Run(count);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private static ActiveDirectoryConfig ParseArgs(IList<string> args, out int count)
        {
            count = default(int);

            if (args.Count != 6)
            {
                return null;
            }

            var server = args[0];
            var distinguishedName = args[1];

            var portString = args[2];
            int port;

            var username = args[3];
            var password = args[4];

            var countString = args[5];

            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(distinguishedName) ||
                string.IsNullOrWhiteSpace(portString) ||
                !Int32.TryParse(portString, out port) ||
                string.IsNullOrWhiteSpace(countString) ||
                !Int32.TryParse(countString, out count) ||
                count < 0 ||
                count > 100 ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            return new ActiveDirectoryConfig(server, distinguishedName, port, username, password);
        }

        private static void ShowUsage()
        {
            Logger.Error("Missing or invalid parameter.");

            Logger.Info("This utility creates a set of test users (max 100) with vanity GUIDs,");
            Logger.Info("starting at {00000000-0000-0000-0000-000000000001}:");

            Logger.Info("\tTestUserCreator.exe <server> <distinguishedName> <port> <username> <password> <count>");

            Logger.Info("The <count> parameter specifies the number of test users to create.");
            Logger.Info("If a test user already exists, they are not re-created, nor is their password reset.");
            Logger.Info("New test users have their password set to \"?Password01!\" (no quotes).");
        }
    }
}
