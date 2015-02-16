namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using System;
    using Builders;
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void ValidationError()
        {
            var viewModel = new LoginViewModelBuilder().Build();

            var mediator = new LoginMediatorBuilder().Build();
            var response = mediator.Index(viewModel);

            response.AssertValidationResult(LoginMediatorCodes.Index.ValidationError);
        }

        [Test]
        public void AccountLocked()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder(UserStatuses.Locked).Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.AccountLocked);
        }

        [Test]
        public void PendingActivation()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var loginResultViewModel = new LoginResultViewModelBuilder(UserStatuses.PendingActivation).Build();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.PendingActivation);
        }

        [Test]
        public void SessionReturnUrl()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "http://return.url.com";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.SessionReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ReturnUrl, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void ReturnUrl()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "http://return.url.com";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ReturnUrl, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void ApprenticeshipApply()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string vacancyId = "1";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(CandidateDataItemNames.LastViewedVacancyId)).Returns(vacancyId);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).Build);
            var entityId = Guid.NewGuid();
            candidateServiceProvider.Setup(p => p.GetCandidate(LoginViewModelBuilder.ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            candidateServiceProvider.Setup(p => p.GetApplicationStatus(entityId, 1)).Returns(ApplicationStatuses.Draft);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ApprenticeshipApply, true, true);
            response.Parameters.Should().Be(vacancyId);
        }

        [Test]
        public void ApprenticeshipDetails()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string vacancyId = "1";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(CandidateDataItemNames.LastViewedVacancyId)).Returns(vacancyId);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).Build);
            var entityId = Guid.NewGuid();
            candidateServiceProvider.Setup(p => p.GetCandidate(LoginViewModelBuilder.ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ApprenticeshipDetails, true, true);
            response.Parameters.Should().Be(vacancyId);
        }

        [Test]
        public void LoginFailed()
        {
            var viewModel = new LoginViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).WithPassword(LoginViewModelBuilder.InvalidPassword).Build();

            const string viewModelMessage = "Invalid Email Address or Password";
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder(UserStatuses.Unknown, false).WithViewModelMessage(viewModelMessage).Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.LoginFailed, true, true);
            response.Parameters.Should().Be(viewModelMessage);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok);
        }

        [Test]
        public void TermsAndConditionsVersion()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "http://return.url.com";
            var configurationManager = new Mock<IConfigurationManager>();
            configurationManager.Setup(x => x.GetAppSetting<string>(It.IsAny<string>())).Returns("2");
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithAcceptedTermsAndConditionsVersion("1").Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).With(configurationManager).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void MobileVerificationRequired()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var loginResultViewModel = new LoginResultViewModelBuilder().MobileVerificationRequired().Build();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok);
            response.ViewModel.MobileVerificationRequired.Should().BeTrue();
        }
    }
}