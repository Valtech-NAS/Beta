

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

        public DashboardBinding(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [Given(@"I have (.*) applications in ""(.*)"" state")]
        public void GivenIHaveApplicationsInState(int numberOfApplications, string state)
        {
            var candidate = new CandidateBuilder(UserEmailAddress)
                    .Build();

            var user = new UserBuilder(UserEmailAddress)
                .Build();

            SetTokens(candidate, user);

            for (var i = 0; i < numberOfApplications; i++)
            {
                var applicationStatus = (ApplicationStatuses)Enum.Parse(typeof(ApplicationStatuses), state);
                var applicationBuilder = new ApplicationBuilder(candidate.EntityId, 
                    UserEmailAddress, applicationStatus);
                applicationBuilder.Build();
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
