namespace SFA.Apprenticeships.Web.Employer.Constants
{
    public class AddressViewModelMessages
    {
        public static class AddressLine1Messages
        {
            public const string LabelText = "Address";
            public const string RequiredErrorText = "Please enter your first line of address.";
            public const string TooLongErrorText = "First line of address mustn’t exceed {0} characters.";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "First line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine2Messages
        {
            public const string LabelText = "Second line (optional)";
            public const string TooLongErrorText = "Second line of address mustn’t exceed {0} characters.";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Second line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AddressLine3Messages
        {
            public const string LabelText = "Third line (optional)";
            public const string TooLongErrorText = "Third line of address mustn’t exceed {0} characters.";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Third line of address " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class CityMessages
        {
            public const string LabelText = "City";
            public const string RequiredErrorText = "Please enter your city.";
            public const string TooLongErrorText = "City mustn’t exceed {0} characters.";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "City " + Whitelists.FreetextWhitelist.ErrorText;
        }
        
        public static class PostcodeMessages
        {
            public const string LabelText = "Postcode";
            public const string RequiredErrorText = "Please enter your postcode.";
            public const string TooLongErrorText = "Postcode mustn’t exceed 8 characters.";
            public const string WhiteListRegularExpression = Whitelists.PostcodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Postcode' " + Whitelists.PostcodeWhitelist.ErrorText;
        }
    }
}