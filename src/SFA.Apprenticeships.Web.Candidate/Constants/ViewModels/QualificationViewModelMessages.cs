﻿namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class QualificationViewModelMessages
    {
        public static class GradeMessages
        {
            public const string RequiredErrorText = "Please enter grade";
            public const string TooLongErrorText = "Grade mustn’t exceed 15 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Grade " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class QualificationTypeMessages
        {
            public const string RequiredErrorText = "Please enter qualification type";
            public const string TooLongErrorText = "Qualification type mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Qualification type " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class SubjectMessages
        {
            public const string RequiredErrorText = "Please enter subject";
            public const string TooLongErrorText = "Subject mustn’t exceed 50 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Subject " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class YearMessages
        {
            public const string RequiredErrorText = "Please enter year";
            public const string MustBeNumericText = "Year must be 4 digits, for example 1990";
            public const string BeforeOrEqualErrorText = "Year mustn’t be in the future";
        }
    }
}