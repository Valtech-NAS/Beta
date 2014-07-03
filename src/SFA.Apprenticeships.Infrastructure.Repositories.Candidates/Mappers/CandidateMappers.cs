namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Mappers
{
    using System;
    using Common.Mappers;
    using Domain.Entities.Candidates;
    using Entities;

    public class CandidateMappers : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<Candidate, MongoCandidate>();
            Mapper.CreateMap<MongoCandidate, Candidate>();
        }
    }
}
