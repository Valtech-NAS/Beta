using MongoDB.Driver;

namespace SFA.Apprenticeships.Infrastructure.Mongo.Common
{
    public interface IMongoAdminClient
    {
        bool IsReplicaSet();

        CommandResult RunCommand(string command);
    }
}