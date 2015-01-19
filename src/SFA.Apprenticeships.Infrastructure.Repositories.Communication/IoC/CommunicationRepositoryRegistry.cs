namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication.IoC
{
    using Domain.Interfaces.Repositories;
    using StructureMap.Configuration.DSL;

    public class CommunicationRepositoryRegistry : Registry
    {
        public CommunicationRepositoryRegistry()
        {
            For<IExpiringDraftRepository>().Use<ExpiringDraftRepository>();
        }
    }
}
