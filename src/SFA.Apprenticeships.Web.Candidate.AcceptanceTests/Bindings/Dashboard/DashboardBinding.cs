namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Dashboard
{
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using System;
    using SpecBind.Helpers;
    using TechTalk.SpecFlow;

    [Binding]
    public class DashboardBinding
    {
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
            var candidate = new CandidateBuilder(BindingData.UserEmailAddress)
                    .Build();
            
            SetTokens(candidate);

            _apprenticeshipApplicationBuilder = new ApprenticeshipApplicationBuilder(candidate.EntityId, BindingData.UserEmailAddress);
            _traineeshipApplicationBuilder = new TraineeshipApplicationBuilder(candidate.EntityId, BindingData.UserEmailAddress);

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
                    .WithVacancyId(i + BindingData.ExistentVacancyId)
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

        private void SetTokens(Candidate candidate)
        {
            // Email.
            _tokenManager.SetToken(BindingData.UserEmailAddressTokenName, candidate.RegistrationDetails.EmailAddress);

            // Password.
            _tokenManager.SetToken(BindingData.PasswordTokenName, BindingData.Password);
        }
    }
}
