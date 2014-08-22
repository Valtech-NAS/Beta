namespace SFA.Apprenticeships.Domain.Entities.UnitTests.User
{
    using System;
    using NUnit.Framework;
    using Users;

    [TestFixture]
    public class UserHelperLockUserTests
    {
        [TestCase]
        public void SetStateLockedShoulNotResetLockStatusRelatedAttributes()
        {
            var logingIncorrectAttempts = 3;
            var user = GetUserWithLockRelatedAttributes(logingIncorrectAttempts);

            var accountLockCode = "123456";
            var accountLockExpiry = DateTime.Now;

            user.SetStateLocked(accountLockCode, accountLockExpiry);

            Assert.That(user.LoginIncorrectAttempts, Is.EqualTo(logingIncorrectAttempts));
            Assert.That(user.AccountUnlockCode, Is.EqualTo(accountLockCode));
            Assert.That(user.AccountUnlockCodeExpiry, Is.EqualTo(accountLockExpiry));
        }

        private static User GetUserWithLockRelatedAttributes(int loginIncorrectAttempts)
        {
            var user = new User
            {
                LoginIncorrectAttempts = loginIncorrectAttempts,
                AccountUnlockCode = "ABCDEF",
                AccountUnlockCodeExpiry = DateTime.Now
            };
            return user;
        }
    }
}
