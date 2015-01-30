namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Users
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class UserRepositoryTests : RepositoryIntegrationTest
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;

        [SetUp]
        public void SetUp()
        {
            _userWriteRepository = Container.GetInstance<IUserWriteRepository>();
            _userReadRepository = Container.GetInstance<IUserReadRepository>();
        }

        [Test, Category("Integration")]
        public void ShouldCreateUser()
        {
            var user = CreateAndSaveUserInMongoDb();
            user.Should().NotBeNull();
        }

        [Test, Category("Integration")]
        public void ShouldCreateAndReadUser()
        {
            var user = CreateAndSaveUserInMongoDb();
            var returnUser = _userReadRepository.Get(user.Username);
            returnUser.Should().NotBeNull();
        }


        private User CreateAndSaveUserInMongoDb()
        {
            var user = CreateUser();
            var actualUser = _userWriteRepository.Save(user);
            return actualUser;
        }

        private static User CreateUser()
        {
            var rnd = new Random();
            var emailSuffix = rnd.Next();
            var email = string.Format("specflowtest{0}@test.test", emailSuffix).ToLower();

            return new User
            {
                EntityId = Guid.NewGuid(),
                ActivateCodeExpiry = DateTime.Now.AddDays(30),
                ActivationCode = "KNU56",
                PasswordResetCode = "",
                PasswordResetCodeExpiry = DateTime.Now.AddDays(7),
                Roles = UserRoles.Candidate,
                Status = UserStatuses.PendingActivation,
                Username = email
            };
        }
    }
}