namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Login;
    using Common.Constants;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        private const string ValidEmailAddress = "user@users.com";
        private const string ValidPassword = "Password01";
        private const string InvalidPassword = "NotPassword01";

        [Test]
        public void ValidationError()
        {
            var viewModel = new LoginViewModel();

            var response = Mediator.Index(viewModel);

            response.AssertValidationResult(Codes.Login.Index.ValidationError);
        }
        
        [Test]
        public void AccountLocked()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel {UserStatus = UserStatuses.Locked});

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.AccountLocked);
        }
        
        [Test]
        public void PendingActivation()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            var loginResultViewModel = new LoginResultViewModel
            {
                IsAuthenticated = true, 
                UserStatus = UserStatuses.PendingActivation
            };
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.PendingActivation);
        }

        [Test]
        public void SessionReturnUrl()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            const string returnUrl = "http://return.url.com";
            UserDataProvider.Setup(p => p.Pop(UserDataItemNames.SessionReturnUrl)).Returns(returnUrl);
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel { IsAuthenticated = true });

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.ReturnUrl, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void ReturnUrl()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            const string returnUrl = "http://return.url.com";
            UserDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel { IsAuthenticated = true });

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.ReturnUrl, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void ApprenticeshipApply()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            const string vacancyId = "1";
            UserDataProvider.Setup(p => p.Pop(UserDataItemNames.LastViewedVacancyId)).Returns(vacancyId);
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel { IsAuthenticated = true, EmailAddress = ValidEmailAddress });
            var entityId = Guid.NewGuid();
            CandidateServiceProvider.Setup(p => p.GetCandidate(ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            CandidateServiceProvider.Setup(p => p.GetApplicationStatus(entityId, 1)).Returns(ApplicationStatuses.Draft);

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.ApprenticeshipApply, true);
            response.Parameters.Should().Be(vacancyId);
        }

        [Test]
        public void ApprenticeshipDetails()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            const string vacancyId = "1";
            UserDataProvider.Setup(p => p.Pop(UserDataItemNames.LastViewedVacancyId)).Returns(vacancyId);
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel { IsAuthenticated = true, EmailAddress = ValidEmailAddress });
            var entityId = Guid.NewGuid();
            CandidateServiceProvider.Setup(p => p.GetCandidate(ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.ApprenticeshipDetails, true);
            response.Parameters.Should().Be(vacancyId);
        }

        [Test]
        public void LoginFailed()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = InvalidPassword
            };

            const string viewModelMessage = "Invalid Email Address or Password";
            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel {ViewModelMessage = viewModelMessage});

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.LoginFailed, true);
            response.Parameters.Should().Be(viewModelMessage);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new LoginViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword
            };

            CandidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModel {IsAuthenticated = true});

            var response = Mediator.Index(viewModel);

            response.AssertCode(Codes.Login.Index.Ok);
        }
    }
}