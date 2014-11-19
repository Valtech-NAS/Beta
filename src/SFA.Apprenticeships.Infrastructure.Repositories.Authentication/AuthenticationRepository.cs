namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication
{
    using System;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Entities;
    using Mongo.Common;
    using NLog;

    public class AuthenticationRepository : GenericMongoClient<MongoUserCredentials>, IAuthenticationRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;

        public AuthenticationRepository(
            IConfigurationManager configurationManager,
            IMapper mapper)
            : base(configurationManager, "Authentication.mongoDB", "userCredentials")
        {
            _mapper = mapper;
        }

        public UserCredentials Get(Guid id)
        {
            Logger.Debug("Calling repository to get UserCredentials for user {0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no UserCredentials with for user \"{0}\"" : "Found UserCredentials for user \"{0}\"";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoUserCredentials, UserCredentials>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public UserCredentials Save(UserCredentials entity)
        {
            Logger.Debug("Calling repository to save UserCredentials for user \"{0}\"", entity.EntityId);

            var mongoEntity = _mapper.Map<UserCredentials, MongoUserCredentials>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved UserCredentials to repository for user \"{0}\"", entity.EntityId);

            return _mapper.Map<MongoUserCredentials, UserCredentials>(mongoEntity);
        }
    }
}