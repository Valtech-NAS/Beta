namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class User : BaseEntity
    {
        public string Username { get; set; }

        public UserStatuses Status { get; set; }

        public UserRoles Roles { get; set; }

        public string ActivationCode { get; set; }

        public string PasswordResetCode { get; set; }
    }
}
