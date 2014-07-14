namespace SFA.Apprenticeships.Application.Authentication
{
    using System;
    using Interfaces.Users;

    public class AuthenticationService : IAuthenticationService
    {
     

        public AuthenticationService()
        {
        }

        public void AuthenticateUser(Guid id, string password)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(Guid id, string password)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(Guid id, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
