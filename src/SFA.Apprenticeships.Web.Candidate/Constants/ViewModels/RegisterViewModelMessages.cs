namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class RegisterViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "Please enter first name";
            public const string TooLongErrorText = "First name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "Please enter last name";
            public const string TooLongErrorText = "Last name mustn’t exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Last name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Enter email address";
            public const string HintText = "You'll need this to sign in to your account. The address you choose will be seen by employers.";
            public const string RequiredErrorText = "Please enter email address";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
            public static string UsernameNotAvailableErrorText = "Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it.";
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Enter mobile phone number";
            public const string HintText = "If you don't have a mobile, enter a landline number.";
            public const string RequiredErrorText = "Please enter phone number";
            public const string LengthErrorText = "Phone number must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Phone number " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Create password";
            public const string RequiredErrorText = "Please enter password";
            public const string LengthErrorText = "Password must be at least 8 characters long, contain upper and lowercase letters and one number";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Password " + Whitelists.PasswordWhitelist.ErrorText;
            public const string PasswordsDoNotMatchErrorText = "Sorry, your passwords don’t match";
        }

        public static class TermsAndConditions
        {
            public const string LabelText = "I accept the <a href='/terms' target='_blank'>terms & conditions</a>";
            public const string MustAcceptTermsAndConditions = "Please accept the terms & conditions";
        }

        public class ConfirmPasswordMessages
        {
            public const string LabelText = "Confirm password";
            public const string RequiredErrorText = "Please confirm password";            
        }
    }
}
