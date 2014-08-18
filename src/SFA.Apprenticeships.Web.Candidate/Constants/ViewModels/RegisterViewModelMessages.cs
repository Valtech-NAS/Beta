namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class RegisterViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "First name";
            public const string RequiredErrorText = "'First name' must be supplied";
            public const string TooLongErrorText = "'First name' must not exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'First name' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Last name";
            public const string RequiredErrorText = "'Last name' must be supplied";
            public const string TooLongErrorText = "'Last name' must not exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Last name' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Enter email address";
            public const string HintText = "You'll need this to sign in to your account. The address you choose will be seen by employers.";
            public const string RequiredErrorText = "'Email address' must be supplied";
            public const string TooLongErrorText = "'Email address' must not exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Email address' " + Whitelists.EmailAddressWhitelist.ErrorText;
            public static string UsernameNotAvailableErrorText = "An account has already been registered with the email address supplied";
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Enter mobile phone number";
            public const string HintText = "If you don't have a mobile, enter a landline number.";
            public const string RequiredErrorText = "'Phone number' must be supplied";
            public const string LengthErrorText = "'Phone number' must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Phone number' " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Create password";
            public const string RequiredErrorText = "'Password' must be supplied";
            public const string LengthErrorText = "'Password' must be at least 8 characters long";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Password' " + Whitelists.PasswordWhitelist.ErrorText;
        }

        public static class TermsAndConditions
        {
            public const string LabelText = "I accept the terms & conditions";
            public const string MustAcceptTermsAndConditions = "You must accept the terms and conditions";
        }
    }
}
