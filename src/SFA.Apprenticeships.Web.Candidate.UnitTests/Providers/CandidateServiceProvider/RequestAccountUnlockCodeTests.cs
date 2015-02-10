namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using Application.Interfaces.Users;
    using Builders;
    using Candidate.ViewModels.Login;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    [TestFixture]
    public class RequestAccountUnlockCodeTests
    {
        private const string EmailAddress = "test@test.com";
        private const string AccountUnlockCode = "ABC123";

        [Test]
        public void AccountInIncorrectState()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(cs => cs.ResendAccountUnlockCode(EmailAddress)).Throws(new CustomException(ErrorCodes.EntityStateError));
            var provider = new CandidateServiceProviderBuilder().With(userAccountService).Build();
            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = provider.RequestAccountUnlockCode(viewModel);

            returnedViewModel.Status.Should().Be(AccountUnlockState.UserInIncorrectState);
        }

        [Test]
        public void AccountNotFound()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(cs => cs.ResendAccountUnlockCode(EmailAddress)).Throws(new CustomException(Application.Interfaces.Users.ErrorCodes.UnknownUserError));
            var provider = new CandidateServiceProviderBuilder().With(userAccountService).Build();
            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = provider.RequestAccountUnlockCode(viewModel);

            returnedViewModel.Status.Should().Be(AccountUnlockState.AccountEmailAddressOrUnlockCodeInvalid);
        }

        [Test]
        public void CustomExceptionError()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(cs => cs.ResendAccountUnlockCode(EmailAddress)).Throws(new CustomException("Unknown"));
            var provider = new CandidateServiceProviderBuilder().With(userAccountService).Build();
            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = provider.RequestAccountUnlockCode(viewModel);

            returnedViewModel.Status.Should().Be(AccountUnlockState.Error);
        }

        [Test]
        public void ExceptionError()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(cs => cs.ResendAccountUnlockCode(EmailAddress)).Throws<Exception>();
            var provider = new CandidateServiceProviderBuilder().With(userAccountService).Build();
            var viewModel = new AccountUnlockViewModelBuilder(EmailAddress, AccountUnlockCode).Build();

            var returnedViewModel = provider.RequestAccountUnlockCode(viewModel);

            returnedViewModel.Status.Should().Be(AccountUnlockState.Error);
        }
    }
}