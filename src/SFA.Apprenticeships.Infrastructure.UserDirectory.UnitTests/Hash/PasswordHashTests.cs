namespace SFA.Apprenticeships.Infrastructure.UserDirectory.UnitTests.Hash
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.UserDirectory.Hash;

    [TestFixture]
    public class PasswordHashTests
    {
        [Test]
        public void ValidatePassword()
        {
            var userId = Guid.NewGuid().ToString();
            const string password = "?Password01!";
            const string secretKey = "$2a$06$DCq7YPn5Rq63x1Lad4cll.";
            var passwordHash = new PasswordHash();

            var hash = passwordHash.Generate(userId, password, secretKey);

            passwordHash.Validate(hash, userId, password, secretKey).Should().BeTrue();
        }
    }
}