namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates
{
    using System;
    using Common.Configuration;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;

    public class CandidateRepository : GenericMongoRepository<MongoCandidate>, ICandidateReadRepository,
        ICandidateWriteRepository
    {
        private readonly IMapper _mapper;

        public CandidateRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Candidates.mongoDB", "candidateDB", "candidates")
        {
            _mapper = mapper;
        }

        public Candidate Get(int id)
        {
            var mongoEntity = Collection.FindOneById(id);

            return _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        public void Delete(int id)
        {
            Collection.Remove(Query<MongoCandidate>.EQ(o => o.Id, id));
        }

        public Candidate Save(Candidate entity)
        {
            var mongoEntity = _mapper.Map<Candidate, MongoCandidate>(entity);

            Collection.Save(mongoEntity);

            return _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }
    }
}
