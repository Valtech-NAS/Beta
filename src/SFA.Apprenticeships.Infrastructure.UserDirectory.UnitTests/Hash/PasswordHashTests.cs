using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.UserDirectory.Hash;

namespace SFA.Apprenticeships.Infrastructure.UserDirectory.UnitTests.Hash
{
    [TestFixture]
    public class PasswordHashTests
    {
        [TestCase("Password01")]
        public void PasswordVerification(string password)
        {
            var hashedPassword = PasswordHash.CreateHash(password);
            PasswordHash.ValidatePassword("NotThePassword", hashedPassword).Should().BeFalse();
            PasswordHash.ValidatePassword(password, hashedPassword).Should().BeTrue();
        }
    }
}