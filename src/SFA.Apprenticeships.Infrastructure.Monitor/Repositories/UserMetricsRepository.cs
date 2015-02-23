namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Users.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class UserMetricsRepository : GenericMongoClient<MongoUser>, IUserMetricsRepository
    {
        public UserMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Users.mongoDB", "users")
        {
        }

        public long GetRegisteredUserCount()
        {
            return Collection.Count();
        }

        public long GetRegisteredAndActivatedUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.ActivationCode == null);
        }
    }
}
