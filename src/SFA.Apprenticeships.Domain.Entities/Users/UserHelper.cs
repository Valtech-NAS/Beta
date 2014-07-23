namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;
    using System.Linq;

    public static class UserHelper
    {
        public static void SetStatePendingActivation(this User user, string activationCode, DateTime expiry)
        {
            ClearUserAttributes(user);

            user.Status = UserStatuses.PendingActivation;
            user.ActivateCodeExpiry = expiry;
            user.ActivationCode = activationCode;
        }

        public static void SetStateLocked(this User user, string accountUnlockCode)
        {
            ClearUserAttributes(user);

            user.Status = UserStatuses.Locked;
            user.AccountUnlockCode = accountUnlockCode;
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
                throw new InvalidOperationException(errorMessage);
            }
        }

        private static void ClearUserAttributes(User user)
        {
            user.AccountUnlockCode = null;
            user.LoginIncorrectAttempts = 0;

            user.ActivationCode = null;
            user.ActivateCodeExpiry = null;

            user.PasswordResetCode = null;
            user.PasswordResetCodeExpiry = null;
            user.PasswordResetIncorrectAttempts = 0;
        }
    }
}
