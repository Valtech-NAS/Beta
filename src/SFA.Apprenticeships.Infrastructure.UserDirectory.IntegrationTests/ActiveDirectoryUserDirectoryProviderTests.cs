namespace SFA.Apprenticeships.Infrastructure.UserDirectory.IntegrationTests
{
    using System;
    using Application.Authentication;
    using NUnit.Framework;
    using IoC;
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
            var username = CreateUsername();
            var succeeded = _service.CreateUser(username, Password);
            Assert.IsTrue(succeeded);
        }

        [Test]
        public void ShouldCreateActiveDirectoryUserAndAuthenticate()
        {
            var username = Guid.NewGuid().ToString();
            var succeeded = _service.CreateUser(username, Password);
            Assert.IsTrue(succeeded);

            var authenticationSucceeded = _service.AuthenticateUser(username, Password);
            Assert.IsTrue(authenticationSucceeded);
        }

        [Test]
        public void ShouldCreateActiveDirectoryUserAndChangePassword()
        {
            var username = Guid.NewGuid().ToString();
            var succeeded = _service.CreateUser(username, Password);
            Assert.IsTrue(succeeded);

            var changePasswordSucceeded = _service.ChangePassword(username, Password, NewPassword);
            Assert.IsTrue(changePasswordSucceeded);
        }


        private static string CreateUsername()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
