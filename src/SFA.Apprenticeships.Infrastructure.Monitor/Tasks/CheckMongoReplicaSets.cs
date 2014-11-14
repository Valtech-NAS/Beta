using System;
using System.Linq;
using MongoDB.Bson;
using NLog;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Infrastructure.Mongo.Common;

namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    public class CheckMongoReplicaSets : IMonitorTask
    {
        private const string MonitorAppSetting = "Monitor.ExpectedReplicaSetCount";
        private const string ReplicaSetGetStatusCommand = "replSetGetStatus";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMongoAdminClient _mongoAdminClient;
        private readonly int _expectedReplicaSetCount;

        public CheckMongoReplicaSets(IMongoAdminClient mongoAdminClient, IConfigurationManager configurationManager)
        {
            _mongoAdminClient = mongoAdminClient;
            _expectedReplicaSetCount = int.Parse(configurationManager.GetAppSetting(MonitorAppSetting));
        }

        public string TaskName
        {
            get { return "Check Mongo Replica Sets"; }
        }

        public void Run()
        {
            var verifyReplicaSets = false;
            var isReplicaSet = _mongoAdminClient.IsReplicaSet();
            if (_expectedReplicaSetCount > 1 && isReplicaSet)
            {
                verifyReplicaSets = true;
                Logger.Info("Replica set members will be verified");
            }
            else if (_expectedReplicaSetCount == 1 && !isReplicaSet)
            {
                Logger.Info("Replica set members will not be verified");
            }
            else
                Logger.Error("{0} config is invalid. ExpectedReplicaSetCount: {1}, IsReplicaSet: {2}", TaskName, _expectedReplicaSetCount, isReplicaSet);

            if (verifyReplicaSets)
            {
                var result = _mongoAdminClient.RunCommand(ReplicaSetGetStatusCommand);
                var members = (BsonArray)result.Response["members"];
                var membersCount = members.Values.Count();
                if (_expectedReplicaSetCount != membersCount)
                {
                    throw new Exception(string.Format("Mongo DB replica set count {0} does not match expected {1}", membersCount, _expectedReplicaSetCount));
                }
                const string memberHealth = "health";
                if (members.Values.Any(m => m[memberHealth] != 1))
                {
                    var unhealthyMembers = string.Join(", ", members.Values.Where(m => m[memberHealth] != 1).Select(m => string.Format("Id: {0}, Name: {1}", m["_id"], m["name"])));
                    throw new Exception(string.Format("The following Mongo DB replica set members appear to be down: {0}", unhealthyMembers));
                }
            }
        }
    }
}