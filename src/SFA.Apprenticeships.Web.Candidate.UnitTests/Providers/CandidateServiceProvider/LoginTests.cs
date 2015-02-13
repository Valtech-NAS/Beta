namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class LoginTests
    {
        [Test]
        public void Us616_Ac7_MobileVerificationRequiredLogin()
        {
            var candidateId = Guid.NewGuid();
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(s => s.GetUserStatus(LoginViewModelBuilder.ValidEmailAddress)).Returns(UserStatuses.Active);
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.Authenticate(LoginViewModelBuilder.ValidEmailAddress, LoginViewModelBuilder.ValidPassword)).Returns(new CandidateBuilder(candidateId).AllowMobile(true).VerifiedMobile(false).Build);
            var provider = new CandidateServiceProviderBuilder().With(candidateService).With(userAccountService).Build();
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var resultViewModel = provider.Login(viewModel);

            resultViewModel.MobileVerificationRequired.Should().BeTrue();
        }
    }
}