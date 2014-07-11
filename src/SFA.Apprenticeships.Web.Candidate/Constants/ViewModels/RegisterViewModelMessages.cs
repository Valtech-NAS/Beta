namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class RegisterViewModelMessages
    {
        public static class FirstnameMessages
        {
            public const string LabelText = "Firstname";
            public const string RequiredErrorText = "'Firstname' must be supplied";
            public const string TooLongErrorText = "'Firstname' must not exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Firstname' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class LastnameMessages
        {
            public const string LabelText = "Lastname";
            public const string RequiredErrorText = "'Lastname' must be supplied";
            public const string TooLongErrorText = "'Lastname' must not exceed 35 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Lastname' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Enter email address";
            public const string HintText = "This will be the main way we contact you. The email address you choose will be seen by employers and will also be your username";
            public const string RequiredErrorText = "'Email address' must be supplied";
            public const string TooLongErrorText = "'Email address' must not exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Email address' " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public static class PhoneNumberMessages
        {
            public const string LabelText = "Enter phone number";
            public const string HintText = "You'll need regular access to your email (for example, to activate your account). Answer 'Yes' and you'll receive notifications via email and SMS.";
            public const string RequiredErrorText = "'Phone number' must be supplied";
            public const string LengthErrorText = "'Phone number' must be between 8 and 16 digits";
            public const string WhiteListRegularExpression = Whitelists.PhoneNumberWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Phone number' " + Whitelists.PhoneNumberWhitelist.ErrorText;
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Create password";
            public const string RequiredErrorText = "'Password' must be supplied";
            public const string TooLongErrorText = "'Password' must not exceed xxx charachers";
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