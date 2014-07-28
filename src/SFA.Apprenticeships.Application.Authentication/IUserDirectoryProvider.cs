namespace SFA.Apprenticeships.Application.Authentication
{
    public interface IUserDirectoryProvider
    {
        bool AuthenticateUser(string username, string password);
        bool CreateUser(string username, string password);
        bool ResetPassword(string username, string newpassword);
        bool ChangePassword(string username, string oldPassword, string newPassword);
    }
}