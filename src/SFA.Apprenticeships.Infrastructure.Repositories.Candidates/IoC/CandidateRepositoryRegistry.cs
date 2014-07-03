namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.IoC
{
    using System;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class CandidateRepositoryRegistry : Registry
    {
        public CandidateRepositoryRegistry()
        {
            For<IMapper>().Use<CandidateMappers>().Name = "CandidateMapper";
            For<ICandidateReadRepository>().Use<CandidateRepository>().Ctor<IMapper>().Named("CandidateMapper");
            For<ICandidateWriteRepository>().Use<CandidateRepository>().Ctor<IMapper>().Named("CandidateMapper");
        }
    }
}
