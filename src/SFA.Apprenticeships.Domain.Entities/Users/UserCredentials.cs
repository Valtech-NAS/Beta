namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using Entities;

    public class UserCredentials : BaseEntity
    {
        public string PasswordHash { get; set; }
    }
}
