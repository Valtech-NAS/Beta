using System;
using MongoDB.Driver;
using NLog;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    public class MongoAdminClient : IMongoAdminClient
    {
        private const string ConnectionStringAppSetting = "Admin.mongoDB";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MongoDatabase _database;

        public MongoAdminClient(IConfigurationManager configurationManager)
        {
            var mongoConnectionString = configurationManager.GetAppSetting(ConnectionStringAppSetting);
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString).GetServer().GetDatabase(mongoDbName);
        }

        public bool IsReplicaSet()
        {
            var result = RunCommand("getCmdLineOpts");
            if (result.Ok)
            {
                var parsed = result.Response["parsed"].ToString();
                var isReplicaSet = parsed.Contains("replSet");
                return isReplicaSet;
            }
            throw new Exception(string.Format("Result of the MongoDB command was not OK {0} {1}", result.Ok, result.Response));
        }

        public CommandResult RunCommand(string command)
        {
            Logger.Info("Running command {0}", command);
            var result = _database.RunCommand(command);
            Logger.Info("Command Result {0} {1}", result.Ok, result.Response);
            
            return result;
        }
    }
}