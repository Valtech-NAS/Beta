namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class QualificationViewModelMessages
    {
        public static class GradeMessages
        {
            public const string RequiredErrorText = "'Grade' must be supplied";
            public const string TooLongErrorText = "'Grade' must not exceed 15 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Grade' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class QualificationTypeMessages
        {
            public const string RequiredErrorText = "'Qualification Type' must be supplied";
            public const string TooLongErrorText = "'Qualification Type' must not exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Qualification Type' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SubjectMessages
        {
            public const string RequiredErrorText = "'Subject' must be supplied";
            public const string TooLongErrorText = "'Subject' must not exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Subject' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class YearMessages
        {
            public const string RequiredErrorText = "'Year' must be supplied";
            public const string MustBeNumericText = "'Year' must be a number";
            public const string BeforeOrEqualErrorText = "'Year' must not be in the future";
        }
    }
}