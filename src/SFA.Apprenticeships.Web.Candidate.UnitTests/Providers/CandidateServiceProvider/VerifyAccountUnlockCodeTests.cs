namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using Builders;
    using Candidate.ViewModels.Login;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyAccountUnlockCodeTests : CandidateServiceProviderTestsBase
    {
        private const string EmailAddress = "test@test.com";
        private const string AccountUnlockCode = "ABC123";

        [Test]
        public void GivenEntityStateError_ThenUserInIncorrectStateIsReturned()
        {
            CandidateService.Setup(cs => cs.UnlockAccount(EmailAddress, AccountUnlockCode)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));

            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = CandidateServiceProvider.VerifyAccountUnlockCode(viewModel);
            returnedViewModel.Status.Should().Be(AccountUnlockState.UserInIncorrectState);
        }
    }
}