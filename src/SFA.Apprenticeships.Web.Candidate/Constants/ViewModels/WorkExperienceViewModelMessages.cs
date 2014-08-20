namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class WorkExperienceViewModelMessages
    {
        public static class DescriptionMessages
        {
            public const string RequiredErrorText = "Please describe your main duties";
            public const string TooLongErrorText = "Main duties mustn’t exceed 200 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Main duties " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmployerMessages
        {
            public const string RequiredErrorText = "Please enter name of employer";
            public const string TooLongErrorText = "Employer name mustn’t exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Employer name " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string RequiredErrorText = "Please enter year started";
            public const string MustBeNumericText = "Please enter year started";
            public const string BeforeOrEqualErrorText = "Year started mustn’t be in the future";
        }

        public static class JobTitleMessages
        {
            public const string RequiredErrorText = "Please enter job title";
            public const string TooLongErrorText = "Job title mustn’t exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Job title " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ToYearMessages
        {
            public const string RequiredErrorText = "Please enter year finished";
            public const string MustBeNumericText = "Year finished must be a number";
            public const string BeforeOrEqualErrorText = "Year finished mustn’t be in the future";
            public const string MustBeFourDigitNumberErrorText = "Year finished must be 4 digits, for example 1990";
        }
    }
}