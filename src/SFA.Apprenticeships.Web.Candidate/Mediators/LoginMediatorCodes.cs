namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class Login
        {
            public class Index
            {
                public const string ValidationError = "Login.Index.ValidationError";
                public const string AccountLocked = "Login.Index.AccountLocked";
                public const string PendingActivation = "Login.Index.PendingActivation";
                public const string ReturnUrl = "Login.Index.ReturnUrl";
                public const string ApprenticeshipApply = "Login.Index.ApprenticeshipApply";
                public const string ApprenticeshipDetails = "Login.Index.ApprenticeshipDetails";
                public const string LoginFailed = "Login.Index.LoginFailed";
                public const string Ok = "Login.Index.Ok";
            }

            public class Unlock
            {
                public const string ValidationError = "Login.Unlock.ValidationError";
                public const string UnlockedSuccessfully = "Login.Unlock.Successfully";
                public const string UserInIncorrectState = "Login.Unlock.UserInIncorrectState";
                public const string AccountEmailAddressOrUnlockCodeInvalid = "Login.Unlock.AccountEmailAddressOrUnlockCodeInvalid";
                public const string AccountUnlockCodeExpired = "Login.Unlock.UnlockCodeExpired";
                public const string AccountUnlockFailed = "Login.Unlock.Failed";
                
            }

            public class Resend
            {
                public const string ValidationError = "Login.Resend.ValidationError";
                public const string ResentSuccessfully = "Login.Resend.Successfully";
                public const string ResendFailed = "Login.Resend.Failed";
            }
        }
    }
}