namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IntegrationTests
{
    using System;
    using Application.Authentication;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class ActiveDirectoryUserDirectoryProviderTests
    {
        private const string Password = "?Passw0rd14";
        private const string NewPassword = "?Passw0rd11";
        private IUserDirectoryProvider _service;

        [SetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(x => x.AddRegistry<UserDirectoryRegistry>());
            _service = ObjectFactory.GetInstance<IUserDirectoryProvider>();
        }

        [Test]
        public void ShouldCreateActiveDirectoryUser()
        {
            string username = CreateUsername();
            bool succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();
        }

        [Test]
        public void ShouldCreateActiveDirectoryUserAndAuthenticate()
        {
            string username = Guid.NewGuid().ToString();
            bool succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();

            bool authenticationSucceeded = _service.AuthenticateUser(username, Password);
            authenticationSucceeded.Should().BeTrue();
        }

        [Test]
        public void ShouldCreateActiveDirectoryUserAndChangePassword()
        {
            string username = Guid.NewGuid().ToString();
            bool succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();

            bool changePasswordSucceeded = _service.ChangePassword(username, Password, NewPassword);
            changePasswordSucceeded.Should().BeTrue();

            bool authenticationSucceeded = _service.AuthenticateUser(username, NewPassword);
            authenticationSucceeded.Should().BeTrue();
        }

        private static string CreateUsername()
        {
            return Guid.NewGuid().ToString();
        }
    }
}