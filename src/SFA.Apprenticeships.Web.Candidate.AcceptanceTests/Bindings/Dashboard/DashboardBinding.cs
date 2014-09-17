namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Dashboard
{
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using System;
    using SpecBind.Helpers;
    using TechTalk.SpecFlow;

    [Binding]
    public class DashboardBinding
    {
        private const string UserEmailAddress = "valtechnas+acceptancetests@gmail.com";
        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string PasswordTokenName = "PasswordToken";
        private const string Password = "?Password01!";
        private readonly ITokenManager _tokenManager;
        private ApplicationBuilder _applicationBuilder;

        public DashboardBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [Given(@"I have an empty dashboard")]
        public void GivenIHaveAnEmptyDashboard()
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                    .Build();

            var user = new UserBuilder(UserEmailAddress)
                .Build();

            SetTokens(candidate, user);

            _applicationBuilder = new ApplicationBuilder(candidate.EntityId,
                    UserEmailAddress);
            _applicationBuilder.DeleteApplications(candidate.EntityId);
        }


        [Given(@"I add (.*) applications in ""(.*)"" state")]
        public void GivenIAddApplicationsInState(int numberOfApplications, string state)
        {
            
            for (var i = 0; i < numberOfApplications; i++)
            {
                var applicationStatus = (ApplicationStatuses)Enum.Parse(typeof(ApplicationStatuses), state);
                _applicationBuilder
                    .WithApplicationStatus(applicationStatus).Build();
            }
        }

        [Given(@"I add (.*) expired applications")]
        public void GivenIAddExpiredApplications(int numberOfApplications)
        {
            for (var i = 0; i < numberOfApplications; i++)
            {
                _applicationBuilder
                    .WithApplicationStatus(ApplicationStatuses.ExpiredOrWithdrawn)
                    .WithExpirationDate(DateTime.Now.AddDays(-5))
                    .WithoutDateApplied()
                    .Build();
            }
        }


        private void SetTokens(Candidate candidate, User user)
        {
            // Email.
            _tokenManager.SetToken(EmailAddressTokenName, candidate.RegistrationDetails.EmailAddress);

            // Password.
            _tokenManager.SetToken(PasswordTokenName, Password);
        }
    }
}
