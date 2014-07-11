namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class DateViewModelMessages
    {
        public const string MustBeValidDate = "You must enter a valid date";

        public static class DayMessages
        {
            public const string LabelText = "Day";
            public const string RequiredErrorText = "'Day' must be supplied";
            public const string RangeErrorText = "'Day' must be between 1 and 31";
        }

        public static class MonthMessages
        {
            public const string LabelText = "Month";
            public const string RequiredErrorText = "'Month' must be supplied";
            public const string RangeErrorText = "'Month' must be between 1 and 12";
        }

        public static class YearMessages
        {
            public const string LabelText = "Year";
            public const string RequiredErrorText = "'Year' must be supplied";
            public const string RangeErrorText = "'Year' must be between {0} and {1}";
        }
    }
}