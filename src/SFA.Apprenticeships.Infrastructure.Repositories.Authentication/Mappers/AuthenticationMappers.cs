namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.Mappers
{
    using Common.Mappers;
    using Entities;

    public class AuthenticationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseUserCredentialsMappers();
        }

        private void InitialiseUserCredentialsMappers()
        {
            Mapper.CreateMap<MongoUserCredentials, UserCredentials>();
        }
    }
}