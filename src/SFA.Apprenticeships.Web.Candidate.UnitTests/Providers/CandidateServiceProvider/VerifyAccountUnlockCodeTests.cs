namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.ViewModels.Login;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyAccountUnlockCodeTests
    {
        private const string EmailAddress = "test@test.com";
        private const string AccountUnlockCode = "ABC123";

        [Test]
        public void GivenEntityStateError_ThenUserInIncorrectStateIsReturned()
        {
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.UnlockAccount(EmailAddress, AccountUnlockCode)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();
            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = provider.VerifyAccountUnlockCode(viewModel);
            
            returnedViewModel.Status.Should().Be(AccountUnlockState.UserInIncorrectState);
        }
    }
}