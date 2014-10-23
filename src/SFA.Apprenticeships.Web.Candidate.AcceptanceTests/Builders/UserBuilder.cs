namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class UserBuilder
    {
        internal static Guid UserAndCandidateId = new Guid("00000000-0000-0000-0000-000000000001");

        public UserBuilder(string username, UserStatuses status = UserStatuses.Active)
        {
            User = new User
            {
                EntityId = UserAndCandidateId,
                Username = username,
                Status = status
            };
        }

        public User User { get; private set; }
 
        public UserBuilder WithActivationCode(string activationCode, DateTime? activationCodeExpiry = null)
        {
            if (!User.ActivateCodeExpiry.HasValue)
            {
                User.ActivateCodeExpiry = activationCodeExpiry ?? DateTime.Now.AddDays(30);
            }

            User.ActivationCode = activationCode;
            
            return this;
        }

        public UserBuilder WithActivationCodeExpiry(DateTime activationCodeExpiry)
        {
            User.ActivateCodeExpiry = activationCodeExpiry;
            
            return this;
        }

        public UserBuilder WithLoginIncorrectAttempts(int loginIncorrectAttempts)
        {
            User.LoginIncorrectAttempts = loginIncorrectAttempts;

            return this;
        }

        public UserBuilder WithPasswordResetCode(string passwordResetCode, DateTime? passwordResetCodeExpiry = null)
        {
            if (!User.PasswordResetCodeExpiry.HasValue)
            {
                User.PasswordResetCodeExpiry = passwordResetCodeExpiry ?? DateTime.Now.AddDays(30);
            }

            User.PasswordResetCode = passwordResetCode;
         
            return this;
        }

        public UserBuilder WithPasswordResetCodeExpiry(DateTime passwordResetCodeExpiry)
        {
            User.PasswordResetCodeExpiry = passwordResetCodeExpiry;
            
            return this;
        }

        public UserBuilder WithAccountUnlockCode(string accountUnlockCode)
        {
            User.AccountUnlockCode = accountUnlockCode;
            
            return this;
        }

        public UserBuilder WithAccountUnlockCodeExpiry(DateTime accountUnlockCodeExpiry)
        {
            User.AccountUnlockCodeExpiry = accountUnlockCodeExpiry;

            return this;
        }

        public User Build()
        {
            var repo = ObjectFactory.GetInstance<IUserWriteRepository>();
            var repoRead = ObjectFactory.GetInstance<IUserReadRepository>();

            if (repoRead.Get(User.Username) == null)
            {
                repo.Save(User);
            }

            return User;
        }
    }
}
