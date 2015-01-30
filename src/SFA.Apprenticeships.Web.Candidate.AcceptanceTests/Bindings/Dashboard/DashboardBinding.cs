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
        private const string UserEmailAddress = "nas.exemplar+acceptancetests@gmail.com";
        private const string EmailAddressTokenName = "EmailAddressToken";
        private const string PasswordTokenName = "PasswordToken";
        private const string Password = "?Password01!";
        private const int ExistentVacancyId = 445650;

        private readonly ITokenManager _tokenManager;

        private ApprenticeshipApplicationBuilder _apprenticeshipApplicationBuilder;
        private TraineeshipApplicationBuilder _traineeshipApplicationBuilder;

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

            _apprenticeshipApplicationBuilder = new ApprenticeshipApplicationBuilder(candidate.EntityId, UserEmailAddress);
            _traineeshipApplicationBuilder = new TraineeshipApplicationBuilder(candidate.EntityId, UserEmailAddress);

            _apprenticeshipApplicationBuilder.DeleteApprenticeshipApplications(candidate.EntityId);
            _traineeshipApplicationBuilder.DeleteTraineeshipApplications(candidate.EntityId);
        }

        [Given(@"I add (.*) applications in ""(.*)"" state")]
        public void GivenIAddApplicationsInState(int numberOfApplications, string state)
        {
            
            for (var i = 0; i < numberOfApplications; i++)
            {
                var applicationStatus = (ApplicationStatuses)Enum.Parse(typeof(ApplicationStatuses), state);
                _apprenticeshipApplicationBuilder
                    .WithVacancyId(i + ExistentVacancyId)
                    .WithApplicationStatus(applicationStatus).Build();
            }
        }

        [Given(@"I add (.*) expired applications")]
        public void GivenIAddExpiredApplications(int numberOfApplications)
        {
            for (var i = 0; i < numberOfApplications; i++)
            {
                _apprenticeshipApplicationBuilder
                    .WithVacancyId(i + 1)
                    .WithApplicationStatus(ApplicationStatuses.ExpiredOrWithdrawn)
                    .WithExpirationDate(DateTime.Now.AddDays(-5))
                    .WithoutDateApplied()
                    .Build();
            }
        }

        [Given(@"I applied for (.*) traineeships")]
        public void GivenIAppliedForTraineeships(int numberOfTraineeships)
        {
            for (var i = 0; i < numberOfTraineeships; i++)
            {
                _traineeshipApplicationBuilder
                    .WithVacancyId(i + 1)
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
