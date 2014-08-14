namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class AboutYouViewModelMessages
    {
        public static class WhatAreYourStrengthsMessages
        {
            public const string LabelText = "Give examples of your strengths that are relevant to the apprenticeship";
            public const string HintText = "Don't just list your strengths. Whatever examples you give, you’ll need to provide details of when you’ve shown these.";
            public const string RequiredErrorText = "'What are your strengths' must be supplied";
            public const string TooLongErrorText = "'What are your strengths' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What are your strengths' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatDoYouFeelYouCouldImproveMessages
        {
            public const string LabelText = "What skills would you like to improve during this apprenticeship?";
            public const string HintText = "Try and think of what your duties will be and the industry the employer works in.";
            public const string RequiredErrorText = "'What do you feel you could improve' must be supplied";
            public const string TooLongErrorText = "'What do you feel you could improve' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What do you feel you could improve' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatAreYourHobbiesInterestsMessages
        {
            public const string LabelText = "What are your hobbies and interests?";
            public const string HintText = "Think of the skills you’ve gained from your hobbies and interests that might be relevant. Include any personal achievements.";
            public const string RequiredErrorText = "'What are your hobbies/interests' must be supplied";
            public const string TooLongErrorText = "'What are your hobbies/interests' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What are your hobbies/interests' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AnythingWeCanDoToSupportYourInterviewMessages
        {
            public const string LabelText = "Is there anything we can do to support your interview?";
            public const string HintText = "Give examples below";
            public const string RequiredErrorText = "'Is there anything we can do to support your interview' must be supplied";
            public const string TooLongErrorText = "'Is there anything we can do to support your interview' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Is there anything we can do to support your interview' " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}