namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    public class RegisterMediatorCodes
    {
        public class Register
        {
            public const string ValidationFailed = "RegisterMediatorCodes.Register.ValidationFailed";
            public const string RegistrationFailed = "RegisterMediatorCodes.Register.RegistrationFailed";
            public const string SuccessfullyRegistered = "RegisterMediatorCodes.Register.SuccessfullyRegistered";
        }

        public class Activate
        {
            public const string SuccessfullyActivated = "RegisterMediatorCodes.Activate.SuccessfullyActivated";
            public const string InvalidActivationCode = "RegisterMediatorCodes.Activate.InvalidActivationCode";
            public const string FailedValidation = "RegisterMediatorCodes.Activate.FailedValidation";
            public const string ErrorActivating = "RegisterMediatorCodes.Activate.ErrorActivating";
        }

        public class ForgottenPassword
        {
            public const string FailedToSendResetCode = "RegisterMediatorCodes.ForgottenPassword.FailedToSendResetCode";
            public const string PasswordSent = "RegisterMediatorCodes.ForgottenPassword.PasswordSent";
            public const string FailedValidation = "RegisterMediatorCodes.ForgottenPassword.FailedValidation";
        }

        public class ResetPassword
        {
            public const string FailedValidation = "RegisterMediatorCodes.ResetPassword.FailedValidation";
            public const string InvalidResetCode = "RegisterMediatorCodes.ResetPassword.InvalidResetCode";
            public const string FailedToResetPassword = "RegisterMediatorCodes.ResetPassword.FailedToResetPassword";
            public const string UserAccountLocked = "RegisterMediatorCodes.ResetPassword.UserAccountLocked";

            public const string SuccessfullyResetPassword = "RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword";
        }
    }
}
