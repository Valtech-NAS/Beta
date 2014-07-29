namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;
    using System.Linq;
    using Exceptions;

    public static class UserHelper
    {
        public static void SetStatePendingActivation(this User user, string activationCode, DateTime expiry)
        {
            ClearUserAttributes(user);

            user.Status = UserStatuses.PendingActivation;
            user.ActivateCodeExpiry = expiry;
            user.ActivationCode = activationCode;
        }

        public static void SetStatePasswordResetCode(this User user, string passwordResetCode, DateTime expiry)
        {
            ClearUserAttributes(user);

            user.PasswordResetCode = passwordResetCode;
            user.PasswordResetCodeExpiry = expiry;
        }

        public static void SetStateLocked(this User user, string accountUnlockCode, DateTime expiry)
        {
            ClearUserAttributes(user);

            user.Status = UserStatuses.Locked;
            user.AccountUnlockCode = accountUnlockCode;
            user.AccountUnlockCodeExpiry = expiry;
        }

        public static void SetStateActive(this User user)
        {
            ClearUserAttributes(user);

            user.Status = UserStatuses.Active;
        }

        public static void AssertState(this User user, string errorMessage, params UserStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(user.Status))
            {
                throw new CustomException(errorMessage, ErrorCodes.UserInIncorrectStateError);
            }
        }

        public static void ClearPasswordResetAttributes(User user)
        {
            user.PasswordResetCode = null;
            user.PasswordResetCodeExpiry = null;
            user.PasswordResetIncorrectAttempts = 0;
        }

        private static void ClearUserAttributes(User user)
        {
            user.AccountUnlockCode = null;
            user.AccountUnlockCodeExpiry = null;

            user.LoginIncorrectAttempts = 0;

            user.ActivationCode = null;
            user.ActivateCodeExpiry = null;

            ClearPasswordResetAttributes(user);
        }
    }
}
