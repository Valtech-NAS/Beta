namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using Candidate.Views.Account;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using FluentValidation.Validators;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardTests
    {
        private IList<MyApprenticeshipApplicationViewModel> _apprenticeshipApplications;
        private IList<MyTraineeshipApplicationViewModel> _traineeshipApplications;
        private TraineeshipFeatureViewModel _traineeshipFeature;

        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SetUpEmptyViewModels();
        }

        private void SetUpEmptyViewModels()
        {
            _apprenticeshipApplications = new List<MyApprenticeshipApplicationViewModel>();
            _traineeshipApplications = new List<MyTraineeshipApplicationViewModel>();
            _traineeshipFeature = new TraineeshipFeatureViewModel();
        }

        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(0, 1, false)]
        public void ShouldShowBigFindApprenticeshipButton(
            int apprenticeshipApplicationCount, int traineeshipApplicationCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(apprenticeshipApplicationCount);
            AddTraineeshipApplications(traineeshipApplicationCount);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("find-apprenticeship-button");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(null, null, false)]
        [TestCase("2", null, false)]
        [TestCase("2", "Chef", true)]
        public void ShouldShowDeletedVacancy(string deletedVacancyId, string deletedVacancyTitle, bool shouldShow)
        {
            // Arrange.
            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature)
            {
                DeletedVacancyId = deletedVacancyId,
                DeletedVacancyTitle = deletedVacancyTitle
            };

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("VacancyDeletedInfoMessageText");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Contain(myApplications.DeletedVacancyId);
                elem.InnerHtml.Should().Contain(myApplications.DeletedVacancyTitle);
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowTraineeshipPrompt(bool shouldShow)
        {
            // Arrange.
            _traineeshipFeature.ShowTraineeshipsPrompt = shouldShow;

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("traineeshipPrompt");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, 0, false)]
        [TestCase(2, 0, true)]
        [TestCase(2, 3, true)]
        public void ShouldShowFindApprenticeshipLink(
            int apprenticeshipApplicationCount, int traineeshipApplicationCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(apprenticeshipApplicationCount);
            AddTraineeshipApplications(traineeshipApplicationCount);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);
            var elem = view.GetElementbyId("find-apprenticeship-link");

            // Assert.
            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowFindTraineeshipLink(bool shouldShow)
        {
            // Arrange.
            _traineeshipFeature.ShowTraineeshipsLink = shouldShow;

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("find-traineeship-link");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowSuccessfulApprenticeshipApplicationsCount(
            int successfulApplicationsCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(successfulApplicationsCount, ApplicationStatuses.Successful);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("successful-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(successfulApplicationsCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, 0, false)]
        [TestCase(2, 0, true)]
        [TestCase(0, 3, true)]
        [TestCase(2, 3, true)]
        public void ShouldShowSubmittedApprenticeshipApplicationsCount(
            int submittingApplicationsCount, int submittedApplicationsCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(submittingApplicationsCount, ApplicationStatuses.Submitting);
            AddApprenticeshipApplications(submittedApplicationsCount, ApplicationStatuses.Submitted);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("submitted-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(
                    submittingApplicationsCount + submittedApplicationsCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowUnsuccessfulApprenticeshipApplicationsCount(
            int unsuccessfulApplicationsCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(unsuccessfulApplicationsCount, ApplicationStatuses.Unsuccessful);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("unsuccessful-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(unsuccessfulApplicationsCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowDraftApprenticeshipApplicationsCount(
            int draftApplicationsCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeshipApplications(draftApplicationsCount, ApplicationStatuses.Draft);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeshipApplications, _traineeshipApplications, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("draft-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(draftApplicationsCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        #region Helpers

        private void AddApprenticeshipApplications(int count, ApplicationStatuses applicationStatus = ApplicationStatuses.Unknown)
        {
            for (var i = 0; i < count; i++)
            {
                _apprenticeshipApplications.Add(new MyApprenticeshipApplicationViewModel
                {
                    ApplicationStatus = applicationStatus
                });
            }
        }

        private void AddTraineeshipApplications(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _traineeshipApplications.Add(new MyTraineeshipApplicationViewModel());
            }
        }

        #endregion
    }
}
