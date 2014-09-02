
namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies.AuthenticateCandidateStrategyTests
{
    using System;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Candidate.Strategies;
    using SFA.Apprenticeships.Application.Interfaces.Users;
    using SFA.Apprenticeships.Application.UserAccount.Strategies;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Domain.Interfaces.Repositories;

    [TestFixture]
    public class GivenAUserWithSomeFailedLoginAttempts
    {
        [TestCase]
        public void WhenTheUserAuthenticatesSuccessfuly_ThenTheFailedLoginAttemptsCounterResetsToZero()
        {
            // Arrange
            var configManager = new Mock<IConfigurationManager>();
            var authenticationService = new Mock<IAuthenticationService>();
            var userReadRepository = new Mock<IUserReadRepository>();
            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateReadRepository = new Mock<ICandidateReadRepository>();
            var lockAccountStrategy = new Mock<ILockAccountStrategy>();
            int maximumPasswordAttemptsAllowed = 3;
            configManager.Setup(cm => cm.GetAppSetting<int>("MaximumPasswordAttemptsAllowed"))
                .Returns(maximumPasswordAttemptsAllowed);

            var user = GetAnActiveUserWithOneLoginIncorrectAttempt();

            userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(), false))
                .Returns(user);

            authenticationService.Setup(auth => auth.AuthenticateUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(true);

            var authenticateCandidateStrategy = new AuthenticateCandidateStrategy(
                configManager.Object, authenticationService.Object, userReadRepository.Object,
                userWriteRepository.Object, candidateReadRepository.Object, lockAccountStrategy.Object);

            // Act
            authenticateCandidateStrategy.AuthenticateCandidate("username", "password");

            //Assert
            userWriteRepository.Verify(uwr => uwr.Save(It.Is<User>(u => u.LoginIncorrectAttempts == 0)));
        }

        private static User GetAnActiveUserWithOneLoginIncorrectAttempt()
        {
            var user = new User();
            user.LoginIncorrectAttempts = 1;
            user.Status = UserStatuses.Active;
            user.EntityId = Guid.NewGuid();
            return user;
        }
    }
}
