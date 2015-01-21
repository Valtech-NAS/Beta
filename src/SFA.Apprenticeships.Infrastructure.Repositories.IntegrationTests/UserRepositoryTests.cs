﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using System;
    using Common.IoC;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using NUnit.Framework;
    using StructureMap;
    using Users.IoC;

    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
            });
#pragma warning restore 0618

            _userWriteRepository = ObjectFactory.GetInstance<IUserWriteRepository>();
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
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