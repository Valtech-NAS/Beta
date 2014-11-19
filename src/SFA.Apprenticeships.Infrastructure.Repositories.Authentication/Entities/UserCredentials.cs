namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.Entities
{
    using Domain.Entities;

    public class UserCredentials : BaseEntity
    {
        public string PasswordHash { get; set; }
    }
}
