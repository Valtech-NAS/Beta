namespace SFA.Apprenticeships.Web.Candidate.Constants.Pages
{
    using System;

    public static class ActivationPageMessages
    {
        public const string ActivationCodeSent = "An activation code has been sent to {0}";
        //TODO: Check mechanism for calling activation complete analytics (see below)
        public const string AccountActivated = "You've successfully activated your account <script>$(function() { Webtrends.multiTrack({ element: this, argsa: ['DCS.dcsuri', '/register/activation/complete', 'WT.dl', '0', 'WT.ti', 'Activation Complete'] }); })</script>";
        public const string ActivationCodeIncorrect = "Activation code entered is incorrect";
        public const string ActivationFailed = "There's been a problem activating your account. Please try again.";
        public const string ActivationCodeSendingFailure = "There's been a problem with your activation code. Please try again.";
    }
}
