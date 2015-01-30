using System;
using MongoDB.Driver;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;

namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    using Application.Interfaces.Logging;

    public class MongoAdminClient : IMongoAdminClient
    {
        private readonly ILogService _logger;
        private const string ConnectionStringAppSetting = "Admin.mongoDB";
        private readonly MongoDatabase _database;

        public MongoAdminClient(IConfigurationManager configurationManager, ILogService logger)
        {
            _logger = logger;
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
            _logger.Info("Running command {0}", command);
            var result = _database.RunCommand(command);
            _logger.Info("Command Result {0} {1}", result.Ok, result.Response);
            
            return result;
        }
    }
}