namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System;

    public static class AccountUnlockViewModelMessages
    {
        public static class AccountUnlockCodeMessages
        {
            public const string LabelText = "Enter code";
            public const string HintText = "";
            public const string RequiredErrorText = "'Account unlock' must be supplied";
            public const string LengthErrorText = "'Account unlock' must be a 6 character code";
            public const string WrongAccountUnlockCodeErrorText = "'Unlock code' supplied is incorrect";
            public const string AccountUnlockedText = "TODO: you successfully unlocked your account";
        }
    }
}
