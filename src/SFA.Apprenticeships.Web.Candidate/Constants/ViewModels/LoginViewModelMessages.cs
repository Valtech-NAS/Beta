namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public class LoginViewModelMessages
    {
        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "'Email address' must be supplied";
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Password";
            public const string RequiredErrorText = "'Password' must be supplied";
        }

        public static class AuthenticationMessages
        {
            public const string AuthenticationFailedErrorText = "'Email address' or 'password' is invalid";
        }
    }
}
