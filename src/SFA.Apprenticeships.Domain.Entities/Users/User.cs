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

        public int LoginRemainingAttempts { get; set; } // used for incorrect password count

        public string PasswordResetCode { get; set; }

        public DateTime? PasswordResetCodeExpiry { get; set; }
        
        public int PasswordResetRemainingAttempts { get; set; } // used for incorrect password reset count

        public string AccountUnlockCode { get; set; }
    }
}
