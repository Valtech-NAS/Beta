namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class EmployerQuestionAnswersViewModelMessages
    {
        public static class CandidateAnswer1Messages
        {
            public const string RequiredErrorText = "The employer question must be supplied";
            public const string TooLongErrorText = "The employer question must not exceed 4000 characters";
            public const string WhitelistRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhitelistErrorText = "The employer question " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class CandidateAnswer2Messages
        {
            public const string RequiredErrorText = "The employer question must be supplied";
            public const string TooLongErrorText = "The employer question must not exceed 4000 characters";
            public const string WhitelistRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhitelistErrorText = "The employer question " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}