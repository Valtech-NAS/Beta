namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class EducationViewModelMessages
    {
        public static class NameOfMostRecentSchoolCollegeMessages
        {
            public const string LabelText = "Name of most recent school or college";
            public const string RequiredErrorText = "'Name of most recent school or college' must be supplied";
            public const string TooLongErrorText = "'Name of most recent school or college' mustn't exceed 120 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Name of most recent school or college' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string LabelText = "From";
            public const string RequiredErrorText = "'From' must be supplied";
            public const string NotInFutureErrorText = "'From' can't be in the future";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'From' " + Whitelists.YearWhitelist.ErrorText;
        }

        public static class ToYearMessages
        {
            public const string LabelText = "To";
            public const string RequiredErrorText = "'From' must be supplied";
            public const string BeforeOrEqualErrorText = "'From' can't be greater than 'To'";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'To' " + Whitelists.YearWhitelist.ErrorText;
        }
    }
}