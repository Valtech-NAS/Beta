namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class AboutYouMessages
    {
        public static class WhatAreYourStrengthsMessages
        {
            public const string LabelText = "What are your strengths?";
            public const string HintText = "For example, team working, organising";
            public const string RequiredErrorText = "'What are your strengths' must be supplied";
            public const string TooLongErrorText = "'What are your strengths' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What are your strengths' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatDoYouFeelYouCouldImproveMessages
        {
            public const string LabelText = "Where do you feel you could improve?";
            public const string HintText = "For example, time managing, confidence";
            public const string RequiredErrorText = "'What do you feel you could improve' must be supplied";
            public const string TooLongErrorText = "'What do you feel you could improve' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What do you feel you could improve' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class WhatAreYourHobbiesInterestsMessages
        {
            public const string LabelText = "What are your hobbies/interests?";
            public const string HintText = "These can include any personal achievements";
            public const string RequiredErrorText = "'What are your hobbies/interests' must be supplied";
            public const string TooLongErrorText = "'What are your hobbies/interests' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'What are your hobbies/interests' " + Whitelists.FreetextWhitelist.ErrorText;
        }

        public static class AnythingWeCanDoToSupportYourInterviewMessages
        {
            public const string LabelText = "Is there anything we can do to support your interview?";
            public const string HintText = "For example, do you need a signer, information in braille";
            public const string RequiredErrorText = "'Is there anything we can do to support your interview' must be supplied";
            public const string TooLongErrorText = "'Is there anything we can do to support your interview' must not exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Is there anything we can do to support your interview' " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}