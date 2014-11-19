namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.IoC
{
    using System;
    using Authentication;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
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
