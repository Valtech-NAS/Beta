namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class ApprenticeshipSearchViewModelMessages
    {
        public static class KeywordMessages
        {
            public const string LabelText = "Keywords (optional)";
            public const string HintText = "Can include job title, employer or reference number";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Keywords' " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LocationMessages
        {
            public const string LabelText = "Location";
            public const string HintText = "Enter postcode, town or city";
            public const string RequiredErrorText = "Please enter location";
            public const string LengthErrorText = "Location must be 3 or more characters or a postcode";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Location " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class CategoryMessages
        {
            public const string RequiredErrorText = "Please select a category";
        }
    }
}