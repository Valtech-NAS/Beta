namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.IoC
{
    using System;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class ApplicationRepositoryRegistry : Registry
    {
        public ApplicationRepositoryRegistry()
        {
            For<IMapper>().Use<ApplicationDetailMappers>().Name = "ApplicationDetailMapper";
            For<IApplicationWriteRepository>().Use<ApplicationRepository>().Ctor<IMapper>("ApplicationDetailMapper");
        }
    }
}
