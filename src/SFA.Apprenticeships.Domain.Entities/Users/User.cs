namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class User : BaseEntity
    {
        private string _username;

        public string Username
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_username))
                {
                    return _username.ToLower();
                }

                return null;
            }
            set
            {
                _username = value;
            }
        }

        public UserStatuses Status { get; set; }

        public UserRoles Roles { get; set; }

        public string ActivationCode { get; set; }

        public DateTime? ActivateCodeExpiry { get; set; }

        public int LoginIncorrectAttempts { get; set; }

        public string PasswordResetCode { get; set; }

        public DateTime? PasswordResetCodeExpiry { get; set; }

        public int PasswordResetIncorrectAttempts { get; set; }

        public string AccountUnlockCode { get; set; }

        public DateTime? AccountUnlockCodeExpiry { get; set; }
    }
}
