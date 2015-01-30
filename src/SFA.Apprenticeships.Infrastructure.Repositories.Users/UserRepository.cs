namespace SFA.Apprenticeships.Infrastructure.Repositories.Users
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using Domain.Entities.Exceptions;

    public class UserRepository : GenericMongoClient<MongoUser>, IUserReadRepository, IUserWriteRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public UserRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "Users.mongoDB", "users")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public User Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get user with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public User Get(string username, bool errorIfNotFound = true)
        {
            _logger.Debug("Called Mongodb to get user with username={0}", username);

            var mongoEntity = Collection.FindOne(Query.EQ("Username", username.ToLower()));

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown username={0}", username);
                _logger.Debug(message, username);

                throw new CustomException(message, Application.Interfaces.Users.ErrorCodes.UnknownUserError);
            }

            return mongoEntity == null ? null : _mapper.Map<MongoUser, User>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        public User Save(User entity)
        {
            _logger.Debug("Called Mongodb to save user with username={0}", entity.Username);

            var mongoEntity = _mapper.Map<User, MongoUser>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved User to Mongodb with username={0}", entity.Username);

            return _mapper.Map<MongoUser, User>(mongoEntity);
        }
    }
}