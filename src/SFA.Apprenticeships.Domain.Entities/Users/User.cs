namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class User : BaseEntity
    {
        public string Username { get; set; }

        public UserStatuses Status { get; set; }

        public UserRoles Roles { get; set; }

        public string ActivationCode { get; set; }

        public DateTime? ActivateCodeExpiry { get; set; }

        public int LoginIncorrectAttempts { get; set; }

        public string PasswordResetCode { get; set; }

        public DateTime? PasswordResetCodeExpiry { get; set; }
        
        public int PasswordResetIncorrectAttempts { get; set; }

        public string AccountUnlockCode { get; set; }
    }
}
