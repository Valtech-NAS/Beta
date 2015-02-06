namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System;
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
            public const string BeforeOrEqualErrorText = "Year started can’t be after year finished";
            public const string RequiredErrorText = "Please enter year started";
            public const string MustBeNumericText = "Please enter year started";
            public const string CanNotBeInTheFutureErrorText = "Year started can’t be in the future";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Year started " + Whitelists.YearWhitelist.ErrorText; 
            public static Func<string, string> MustBeGreaterThan = year => "Year must be 4 digits, and not before " + year;
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
            public const string MustBeNumericText = "Year finished must be a number";
            public const string WhiteListRegularExpression = Whitelists.YearWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Year finished " + Whitelists.YearWhitelist.ErrorText;
            public static Func<string, string> MustBeGreaterThan = year => "Year must be 4 digits, and not before " + year;
            public static string CanNotBeInTheFutureErrorText = "Year finished can’t be in the future";
        }
    }
}