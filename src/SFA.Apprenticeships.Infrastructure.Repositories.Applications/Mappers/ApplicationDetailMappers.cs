namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Mappers
{
    using System;
    using Common.Mappers;
    using Domain.Entities.Candidates;
    using Entities;

    public class ApplicationDetailMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Candidate, MongoApplicationDetail>();
            Mapper.CreateMap<MongoApplicationDetail, Candidate>();
        }
    }
}
