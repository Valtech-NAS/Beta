namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;

    public class CandidateRepository : GenericMongoClient<MongoCandidate>, ICandidateReadRepository,
        ICandidateWriteRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public CandidateRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "Candidates.mongoDB", "candidates")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public Candidate Get(Guid id)
        {
            _logger.Debug("Calling repository to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            LogOutcome(id, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            _logger.Debug("Calling repository to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with Id={0}", id);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            LogOutcome(id, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public Candidate Get(string username, bool errorIfNotFound = true)
        {
            _logger.Debug("Calling repository to get candidate with username={0}", username);

            var mongoEntity = Collection.FindOne(Query<MongoCandidate>.EQ(o => o.RegistrationDetails.EmailAddress, username.ToLower()));

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with EmailAddress={0}", username);
                _logger.Debug(message, username);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError); 
            }

            LogOutcome(username, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete candidate with Id={0}", id);

            Collection.Remove(Query<MongoCandidate>.EQ(o => o.Id, id));

            _logger.Debug("Deleted candidate with Id={0}",id);
        }

        public Candidate Save(Candidate entity)
        {
            _logger.Debug("Calling repository to save candidate with Id={0}, FirstName={1}, EmailAddress={2}", entity.EntityId, entity.RegistrationDetails.FirstName, entity.RegistrationDetails.EmailAddress);

            var mongoEntity = _mapper.Map<Candidate, MongoCandidate>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved candidate to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        private void LogOutcome(Guid id, MongoCandidate mongoEntity)
        {
            var message = mongoEntity == null ? "Found no candidate with Id={0}" : "Found candidate with Id={0}";

            _logger.Debug(message, id);
        }

        private void LogOutcome(string username, MongoCandidate mongoEntity)
        {
            var message = mongoEntity == null ? "Found no candidate with username={0}" : "Found candidate with username={0}";

            _logger.Debug(message, username);
        }

        private Candidate CandidateOrNull(MongoCandidate mongoEntity)
        {
            if (mongoEntity == null) return null;

            var entity = _mapper.Map<MongoCandidate, Candidate>(mongoEntity);

            return entity;
        }
    }
}