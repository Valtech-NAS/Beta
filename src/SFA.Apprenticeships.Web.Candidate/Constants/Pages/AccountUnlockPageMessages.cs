namespace SFA.Apprenticeships.Web.Candidate.Constants.Pages
{
    using System;

    public static class AccountUnlockPageMessages
    {
        public const string AccountUnlockFailed = "There’s been an error unlocking your account. Please try again.";
        public const string AccountUnlockCodeExpired = "The code to unlock your account has expired. Please use the new one we’ve sent you.";
        public const string WrongEmailAddressOrAccountUnlockCodeErrorText = "Please enter a valid email address or unlock account code";
        public const string AccountUnlockedText = "You've successfully unlocked your account";
        public const string AccountUnlockCodeMayHaveBeenResent = "Please check your email for an account unlock code. If you do not recieve one, please check you entered your email correctly and that your account is locked.";
        public const string AccountUnlockResendCodeFailed = "There’s been a problem sending you a code to unlock your account. Please try again.";
    }
}
