namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class VacancySearchMessages
    {
        public static class KeywordMessages
        {
            public const string LabelText = "Keywords (optional)";
            public const string HintText = "For example, mechanical engineer, retail, customer service";
            public const string WhiteList = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Keywords' " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LocationMessages
        {
            public const string LabelText = "Apprenticeship location";
            public const string HintText = "Enter postcode, town or city";
            public const string RequiredErrorText = "Please provide a location.";
            public const string LengthErrorText = "Location name or postcode must be 3 or more characters.";
            public const string NoResultsErrorText = "Sorry, we didn't find a match for the location entered";
            public const string WhiteList = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Apprenticeship location' " + Whitelists.NameWhitelist.ErrorText;
        }
    }
}