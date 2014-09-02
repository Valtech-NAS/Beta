namespace SFA.Apprenticeships.Infrastructure.Repositories.Users
{
    using System;
    using System.Linq;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NLog;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;

    public class UserRepository : GenericMongoClient<MongoUser>, IUserReadRepository, IUserWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;


        public UserRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Users.mongoDB", "users")
        {
            _mapper = mapper;
        }

        public User Get(Guid id)
        {
            Logger.Debug("Called Mongodb to get user with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public User Get(string username, bool errorIfNotFound = true)
        {
            Logger.Debug("Called Mongodb to get user with username={0}", username);

            var mongoEntity = Collection.FindOne(Query.EQ("Username", username.ToLower()));

            if (mongoEntity == null && errorIfNotFound)
            {
                Logger.Debug("Unknown username={0}", username);

                throw new CustomException("Unknown candidate", ErrorCodes.UnknownUserError);
            }

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        public User Save(User entity)
        {
            Logger.Debug("Called Mongodb to save user with username={0}", entity.Username);

            var mongoEntity = _mapper.Map<User, MongoUser>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved User to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoUser, User>(mongoEntity);
        }

        protected override void Initialise()
        {
            Collection.CreateIndex(new IndexKeysBuilder().Ascending("Username"), IndexOptions.SetUnique(true));
        }
    }
}