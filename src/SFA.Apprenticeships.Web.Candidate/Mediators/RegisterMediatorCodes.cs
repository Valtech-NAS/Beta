namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    public partial class Codes
    {
        public class RegisterMediatorCodes
        {
            public class Register
            {
                public const string ValidationFailed = "ValidationFailed";
                public const string RegistrationFailed = "RegistrationFailed";
                public const string SuccessfullyRegistered = "SuccessfullyRegistered";
            }

            public class Activate
            {
                public const string SuccessfullyActivated = "SuccessfullyActivated";
                public const string InvalidActivationCode = "InvalidActivationCode";
                public const string FailedValidation = "FailedValidation";
                public const string ErrorActivating = "ErrorActivating";
            }

            public class ForgotttenPassword
            {
                public const string FailedToSendResetCode = "FailedToSendResetCode";
                public const string PasswordSent = "PasswordSent";
                public const string FailedValidation = "FailedValidation";
            }

            public class ResetPassword
            {
                public const string FailedValidation = "FailedValidation";
                public const string InvalidResetCode = "InvalidResetCode";
                public const string FailedToResetPassword = "FailedToResetPassword";
                public const string UserAccountLocked = "UserAccountLocked";
                public const string SuccessfullyResetPassword = "SuccessfullyResetPassword";
            }
        }
    }
}