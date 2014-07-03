namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class User : BaseEntity
    {
        //todo: User entity, status, etc.

        public UserRoles Roles { get; set; }
    }
}
