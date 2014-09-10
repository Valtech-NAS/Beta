namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using NLog;

    public class CandidateRepository : GenericMongoClient<MongoCandidate>, ICandidateReadRepository,
        ICandidateWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;

        public CandidateRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Candidates.mongoDB", "candidates")
        {
            _mapper = mapper;
        }

        public Candidate Get(Guid id)
        {
            Logger.Debug("Called Mongodb to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);
            return mongoEntity == null ? null : _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            Logger.Debug("Called Mongodb to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with Id={0}", id);
                Logger.Debug(message);

                throw new CustomException(message, ErrorCodes.UnknownCandidateError);
            }

            return mongoEntity == null ? null : _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        public Candidate Get(string username, bool errorIfNotFound = true)
        {
            Logger.Debug("Called Mongodb to get candidate with username={0}", username);

            var mongoEntity = Collection.FindOne(Query<MongoCandidate>.EQ(o => o.RegistrationDetails.EmailAddress, username.ToLower()));

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with EmailAddress={0}", username);
                Logger.Debug(message, username);

                throw new CustomException(message, ErrorCodes.UnknownCandidateError); 
            }

            return mongoEntity == null ? null : _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Called Mongodb to delete candidate with Id={0}", id);

            Collection.Remove(Query<MongoCandidate>.EQ(o => o.Id, id));
        }

        public Candidate Save(Candidate entity)
        {
            Logger.Debug("Called Mongodb to save candidate EntityId={0}, FirstName={1}, EmailAddress={2}", entity.EntityId, entity.RegistrationDetails.FirstName, entity.RegistrationDetails.EmailAddress);
            
            var mongoEntity = _mapper.Map<Candidate, MongoCandidate>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved candidate to Mongodb with Id={0}", entity.EntityId);

            return _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        protected override void Initialise()
        {
            //TODO: review the index
            Collection.CreateIndex(new IndexKeysBuilder().Ascending("RegistrationDetails.EmailAddress"), IndexOptions.SetUnique(true));
        }
    }
}