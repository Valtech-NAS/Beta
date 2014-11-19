namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.IoC
{
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Infrastructure.Repositories.Authentication.Mappers;
    using StructureMap.Configuration.DSL;

    public class AuthenticationRepositoryRegistry : Registry
    {
        public AuthenticationRepositoryRegistry()
        {
            For<IMapper>().Use<AuthenticationMappers>().Name = "AuthenticationMappers";

            For<IAuthenticationRepository>()
                .Use<AuthenticationRepository>()
                .Ctor<IMapper>()
                .Named("AuthenticationMappers");
        }
    }
}