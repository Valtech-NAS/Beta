namespace SFA.Apprenticeships.Application.UnitTests.UserAccount
{
    using System;
    using Application.UserAccount.Strategies;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;
    using FluentAssertions;

    [TestFixture]
    public class UnlockAccountStrategyTests
    {
        private const string Username = "username";
        private const string UnlockAccountCode = "account unlock code";
        private const string AnotherAccountUnlockCode = "another unlock code";

        [Test]
        public void RenewUnlockAccount()
        {
            var userReadRepository = new Mock<IUserReadRepository>();
            var sendAccountUnlockCodeStrategy = new Mock<ISendAccountUnlockCodeStrategy>();

            userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(),true)).Returns(new User
            {
                AccountUnlockCodeExpiry = DateTime.Now.AddDays(-1),
                Status = UserStatuses.Locked
            });

            var unlockAccountStrategy = new UnlockAccountStrategy(userReadRepository.Object, null,
                sendAccountUnlockCodeStrategy.Object);

            Action a = () => unlockAccountStrategy.UnlockAccount(Username, UnlockAccountCode);
            a.ShouldThrow<CustomException>()
                .WithMessage("Account unlock code has expired, new account unlock code has been sent.");
        }

        [Test]
        public void InvalidUnlockAccountCode()
        {
            var userReadRepository = new Mock<IUserReadRepository>();
            var sendAccountUnlockCodeStrategy = new Mock<ISendAccountUnlockCodeStrategy>();

            userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(), true)).Returns(new User
            {
                AccountUnlockCodeExpiry = DateTime.Now.AddDays(1),
                Status = UserStatuses.Locked,
                AccountUnlockCode = AnotherAccountUnlockCode
            });

            var unlockAccountStrategy = new UnlockAccountStrategy(userReadRepository.Object, null,
                sendAccountUnlockCodeStrategy.Object);

            Action a = () => unlockAccountStrategy.UnlockAccount(Username, UnlockAccountCode);
            a.ShouldThrow<CustomException>()
                .WithMessage(string.Format("Account unlock code \"{0}\" is invalid for user \"{1}\"", UnlockAccountCode, Username));
        }

        [Test]
        public void UnlockAccount()
        {
            var userReadRepository = new Mock<IUserReadRepository>();
            var sendAccountUnlockCodeStrategy = new Mock<ISendAccountUnlockCodeStrategy>();
            var userWriteRepository = new Mock<IUserWriteRepository>();

            userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(), true)).Returns(new User
            {
                AccountUnlockCodeExpiry = DateTime.Now.AddDays(1),
                Status = UserStatuses.Locked,
                AccountUnlockCode = UnlockAccountCode
            });

            var unlockAccountStrategy = new UnlockAccountStrategy(userReadRepository.Object, userWriteRepository.Object,
                sendAccountUnlockCodeStrategy.Object);

            unlockAccountStrategy.UnlockAccount(Username, UnlockAccountCode);
            
            userWriteRepository.Verify(uwr => uwr.Save(It.Is<User>(u =>
                u.AccountUnlockCode == null && 
                u.AccountUnlockCodeExpiry == null && 
                u.LoginIncorrectAttempts == 0 && 
                u.Status == UserStatuses.Active
            )));
        }
    }
}