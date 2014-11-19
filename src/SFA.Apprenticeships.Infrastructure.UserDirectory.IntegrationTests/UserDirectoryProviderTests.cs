namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IntegrationTests
{
    using System;
    using Application.Authentication;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.Repositories.Authentication.IoC;
    using StructureMap;

    public class UserDirectoryProviderTests
    {
        private const string Password = "?Passw0rd14";
        private const string NewPassword = "?Passw0rd11";
        private IUserDirectoryProvider _service;

        [SetUp]
        public void Setup()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
            });
                                            
#pragma warning restore 0618

            _service = ObjectFactory.GetInstance<IUserDirectoryProvider>();
        }

        [Test, Category("Integration")]
        public void ShouldCreateActiveDirectoryUser()
        {
            string username = CreateUseId();
            bool succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();
        }

        [Test, Category("Integration")]
        public void ShouldCreateActiveDirectoryUserAndAuthenticate()
        {
            string username = Guid.NewGuid().ToString();
            bool succeeded = _service.CreateUser(username, Password);
            succeeded.Should().BeTrue();

            bool authenticationSucceeded = _service.AuthenticateUser(username, Password);
            authenticationSucceeded.Should().BeTrue();
        }

        [Test, Category("Integration")]
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

        private static string CreateUseId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}