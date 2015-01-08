namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class TraineeshipSearchViewModelMessages
    {

        public static class LocationMessages
        {
            public const string LabelText = "Your location";
            public const string HintText = "Enter postcode, town or city";
            public const string RequiredErrorText = "Please enter location";
            public const string LengthErrorText = "Location must be 3 or more characters or a postcode";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Location " + Whitelists.NameWhitelist.ErrorText;
        }
    }
}