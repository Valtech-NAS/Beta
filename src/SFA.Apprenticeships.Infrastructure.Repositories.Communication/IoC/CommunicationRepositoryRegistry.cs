namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication.IoC
{
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class CommunicationRepositoryRegistry : Registry
    {
        public CommunicationRepositoryRegistry()
        {
            For<IMapper>().Use<CommunicationMappers>().Name = "CommunicationMappers";
            For<IExpiringDraftRepository>()
                .Use<ExpiringDraftRepository>()
                .Ctor<CommunicationMappers>()
                .Named("CommunicationMappers");
        }
    }
}
