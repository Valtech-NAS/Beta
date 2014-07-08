namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class EducationMessages
    {
        public static class NameOfMostRecentSchoolCollegeMessages
        {
            public const string LabelText = "Name of most recent school/college";
            public const string RequiredErrorText = "'Name of most recent school/college' must be supplied";
            public const string TooLongErrorText = "'Name of most recent school/college' must not exceed 120 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Name of most recent school/college' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string LabelText = "From";
            public const string RequiredErrorText = "'From' must be supplied";
            public const string NotInFutureErrorText = "'From' must not be in the future";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'From' " + Whitelists.YearWhitelist.ErrorText;
        }

        public static class ToYearMessages
        {
            public const string LabelText = "To";
            public const string RequiredErrorText = "'To' must be supplied";
            public const string BeforeOrEqualErrorText = "'From' cannot be greater than the 'To' year";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'To' " + Whitelists.YearWhitelist.ErrorText;
        }
    }
}