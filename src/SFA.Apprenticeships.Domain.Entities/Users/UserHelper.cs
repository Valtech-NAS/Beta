namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;
    using System.Linq;
    using Exceptions;

    public static class UserHelper
    {
        public static void SetStatePendingActivation(this User user, string activationCode, DateTime expiry)
        {
            ClearUserStateAttributes(user);

            user.Status = UserStatuses.PendingActivation;
            user.ActivateCodeExpiry = expiry;
            user.ActivationCode = activationCode;
        }

        public static void SetStatePasswordResetCode(this User user, string passwordResetCode, DateTime expiry)
        {
            user.PasswordResetCode = passwordResetCode;
            user.PasswordResetCodeExpiry = expiry;
        }

        public static void SetStateLocked(this User user, string accountUnlockCode, DateTime expiry)
        {
            ClearNonRelatedToLockStateUserStateAttributes(user);

            user.Status = UserStatuses.Locked;
            user.AccountUnlockCode = accountUnlockCode;
            user.AccountUnlockCodeExpiry = expiry;
        }

        public static void SetStateActive(this User user)
        {
            ClearUserStateAttributes(user);

            user.Status = UserStatuses.Active;
        }

        public static void AssertState(this User user, string errorMessage, params UserStatuses[] allowedUserStatuses)
        {
            if (!allowedUserStatuses.Contains(user.Status))
            {
                var expectedStatuses = string.Join(", ", allowedUserStatuses);
                var message = string.Format("User in invalid state for '{0}' (id: {1}, current: {2}, expected: '{3}')", 
                    errorMessage, 
                    user.EntityId, 
                    user.Status,
                    expectedStatuses);

                throw new CustomException(message, ErrorCodes.EntityStateError);
            }
        }

        private static void ClearNonRelatedToLockStateUserStateAttributes(User user)
        {
            user.ActivationCode = null;
            user.ActivateCodeExpiry = null;

            user.PasswordResetCode = null;
            user.PasswordResetCodeExpiry = null;
            user.PasswordResetIncorrectAttempts = 0;
        }

        private static void ClearUserStateAttributes(User user)
        {
            ClearNonRelatedToLockStateUserStateAttributes(user);

            user.LoginIncorrectAttempts = 0;
            user.AccountUnlockCode = null;
            user.AccountUnlockCodeExpiry = null;
        }
    }
}
