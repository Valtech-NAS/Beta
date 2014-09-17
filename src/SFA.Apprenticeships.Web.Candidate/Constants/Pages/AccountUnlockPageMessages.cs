namespace SFA.Apprenticeships.Web.Candidate.Constants.Pages
{
    using System;

    public static class AccountUnlockPageMessages
    {
        public const string AccountUnlockFailed = "There’s been an error unlocking your account. Please try again.";
        public const string AccountUnlockCodeExpired = "The code to unlock your account has expired. Please use the new one we’ve sent you.";
        public const string WrongAccountUnlockCodeErrorText = "Please enter the correct unlock code. Check your email to make sure you’ve got the correct one.";
        public const string AccountUnlockedText = "You've successfully unlocked your account";
        public const string AccountUnlockCodeResent = "A code to unlock your account has been sent to {0}";
        public const string AccountUnlockResendCodeFailed = "There’s been a problem sending you a code to unlock your account. Please try again.";
    }
}
