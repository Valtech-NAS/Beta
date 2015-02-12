namespace SFA.Apprenticeships.Web.Candidate.Constants.Pages
{
    using System;

    public static class AccountUnlockPageMessages
    {
        public const string AccountUnlockFailed = "There’s been an error unlocking your account. Please try again.";
        public const string AccountUnlockCodeExpired = "The code to unlock your account has expired. Please use the new one we’ve sent you.";
        public const string WrongEmailAddressOrAccountUnlockCodeErrorText = "You may have entered an invalid email address or unlock code.  Please try again.";
        public const string AccountUnlockedText = "You've successfully unlocked your account";
        public const string AccountUnlockCodeMayHaveBeenResent = "We have emailed you an unlock account code.  If you don't receive it you may have entered an incorrect email address or your account may not be locked.";
        public const string AccountUnlockResendCodeFailed = "There’s been a problem sending you a code to unlock your account. Please try again.";
    }
}
