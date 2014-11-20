﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication
{
    using System;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Entities;
    using Mongo.Common;
    using NLog;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using UsersErrorCodes = SFA.Apprenticeships.Application.Interfaces.Users.ErrorCodes;

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

            LogOutcome(id, mongoEntity);

            return mongoEntity == null ? null : _mapper.Map<MongoUserCredentials, UserCredentials>(mongoEntity);
        }

        public UserCredentials Get(Guid id, bool errorIfNotFound)
        {
            Logger.Debug("Calling repository to get UserCredentials for user \"{0}\"", id);

            var mongoEntity = Collection.FindOneById(id);

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown userCredentials for user with Id=\"{0}\"", id);
                Logger.Debug(message);

                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountDoesNotExistError);
            }

            LogOutcome(id, mongoEntity);

            return mongoEntity == null ? null : _mapper.Map<MongoUserCredentials, UserCredentials>(mongoEntity);
        }

        private static void LogOutcome(Guid id, MongoUserCredentials mongoEntity)
        {
            var message = mongoEntity == null
                ? "Found no UserCredentials with for user \"{0}\""
                : "Found UserCredentials for user \"{0}\"";

            Logger.Debug(message, id);
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