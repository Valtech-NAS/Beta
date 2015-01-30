namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IntegrationTests
{
    using System;
    using Application.Authentication;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using Common.IoC;
    using Repositories.Authentication.IoC;
    using StructureMap;

    public class UserDirectoryProviderTests
    {
        private const string Password = "?Passw0rd14";
        private const string NewPassword = "?Passw0rd11";
        private IUserDirectoryProvider _service;

        [SetUp]
        public void Setup()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
            });
                                            
            _service = container.GetInstance<IUserDirectoryProvider>();
        }

        [Test, Category("Integration")]
        public void ShouldCreateUser()
        {
            var username = CreateUserId();
            var succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();
        }

        [Test, Category("Integration")]
        public void ShouldCreateUserAndAuthenticate()
        {
            var username = Guid.NewGuid().ToString();
            var succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();

            var authenticationSucceeded = _service.AuthenticateUser(username, Password);
            authenticationSucceeded.Should().BeTrue();
        }

        [Test, Category("Integration")]
        public void ShouldCreateUserAndChangePassword()
        {
            var username = Guid.NewGuid().ToString();
            var succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();

            var changePasswordSucceeded = _service.ChangePassword(username, Password, NewPassword);
            changePasswordSucceeded.Should().BeTrue();

            var authenticationSucceeded = _service.AuthenticateUser(username, NewPassword);
            authenticationSucceeded.Should().BeTrue();
        }

        private static string CreateUserId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}