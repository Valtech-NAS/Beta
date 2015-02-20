namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.ViewModels.Account;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void SuccessTest()
        {
            var accountProviderMock = new Mock<IAccountProvider>();
            accountProviderMock.Setup(x => x.GetSettingsViewModel(It.IsAny<Guid>())).Returns(new SettingsViewModel());
            var accountMediator = new AccountMediatorBuilder().With(accountProviderMock.Object).Build();

            var response = accountMediator.Settings(Guid.NewGuid());
            response.AssertCode(AccountMediatorCodes.Settings.Success);
        }
    }
}