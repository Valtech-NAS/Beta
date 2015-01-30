namespace SFA.Apprenticeships.Application.Authentication
{
    public interface IUserDirectoryProvider
    {
        bool AuthenticateUser(string userId, string password);

        bool CreateUser(string userId, string password);

        bool ResetPassword(string userId, string newpassword);

        bool ChangePassword(string userId, string oldPassword, string newPassword);
    }
}
