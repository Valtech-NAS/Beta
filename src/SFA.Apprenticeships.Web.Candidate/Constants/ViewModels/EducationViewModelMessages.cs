namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System;
    using Common.Constants;

    public static class EducationViewModelMessages
    {
        public static class NameOfMostRecentSchoolCollegeMessages
        {
            public const string LabelText = "Name of most recent school or college";
            public const string RequiredErrorText = "Please enter name of most recent school or college";
            public const string TooLongErrorText = "Name of most recent school or college mustn't exceed 120 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Name of most recent school or college " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class FromYearMessages
        {
            public const string LabelText = "From";
            public const string RequiredErrorText = "Please enter year started";
            public const string NotInFutureErrorText = "Year started can’t be in the future";
            public static string WhiteListRegularExpression = Whitelists.YearRangeWhiteList.RegularExpression();
            public static string WhiteListErrorText = "Year started " + Whitelists.YearRangeWhiteList.ErrorText();
        }

        public static class ToYearMessages
        {
            public const string LabelText = "To";
            public const string RequiredErrorText = "Please enter year finished";
            public const string BeforeOrEqualErrorText = "Year started can’t be after year finished";
            public static string WhiteListRegularExpression = Whitelists.YearRangeWhiteList.RegularExpression();
            public static string WhiteListErrorText = "Year finished " + Whitelists.YearRangeWhiteList.ErrorText();
        }
    }
}