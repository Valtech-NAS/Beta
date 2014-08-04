namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Builders
{
    using System;
    using Application.Authentication;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class UserBuilder
    {
        public UserBuilder(string userId, string username, UserStatuses status = UserStatuses.Active)
        {
            User = new User
            {
                EntityId = new Guid(userId),
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

            repo.Save(User);

            return User;
        }
    }
}
