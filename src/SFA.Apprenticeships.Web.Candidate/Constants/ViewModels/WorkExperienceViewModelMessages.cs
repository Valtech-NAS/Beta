namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class WorkExperienceViewModelMessages
    {
        public static class DescriptionMessages
        {
            public const string RequiredErrorText = "'Description' must be supplied";
            public const string TooLongErrorText = "'Description' must not exceed 200 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Description' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class EmployerMessages
        {
            public const string RequiredErrorText = "'Employer' must be supplied";
            public const string TooLongErrorText = "'Employer' must not exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Employer' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string RequiredErrorText = "'From Year' must be supplied";
            public const string MustBeNumericText = "'From Year' must be a number";
            public const string BeforeOrEqualErrorText = "'From Year' must not be in the future";
        }

        public static class JobTitleMessages
        {
            public const string RequiredErrorText = "'Job Title' must be supplied";
            public const string TooLongErrorText = "'Job Title' must not exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Job Title' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class ToYearMessages
        {
            public const string RequiredErrorText = "'To Year' must be supplied";
            public const string MustBeNumericText = "'To Year' must be a number";
            public const string BeforeOrEqualErrorText = "'To Year' must not be in the future";
            public const string MustBeFourDigitNumberErrorText = "'To Year' must be a 4 digit number";
        }
    }
}