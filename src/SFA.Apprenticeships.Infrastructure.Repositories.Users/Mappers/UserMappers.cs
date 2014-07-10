namespace SFA.Apprenticeships.Infrastructure.Repositories.Users.Mappers
{
    using System;
    using Common.Mappers;
    using Domain.Entities.Users;
    using Entities;

    public class UserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<User, MongoUser>();
            Mapper.CreateMap<MongoUser, User>();
        }
    }
}
