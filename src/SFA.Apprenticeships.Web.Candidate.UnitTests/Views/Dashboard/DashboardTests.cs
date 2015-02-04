namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using Candidate.Views.Account;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardTests
    {
        private IList<MyApprenticeshipApplicationViewModel> _apprenticeships;
        private IList<MyTraineeshipApplicationViewModel> _traineeships;
        private TraineeshipFeatureViewModel _traineeshipFeature;

        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SetUpEmptyViewModels();
        }

        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(0, 1, false)]
        public void ShouldShowFindApprenticeshipButton(int apprenticeshipCount, int traineeshipCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(apprenticeshipCount);
            AddTraineeships(traineeshipCount);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

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
                _apprenticeships, _traineeships, _traineeshipFeature)
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
                _apprenticeships, _traineeships, _traineeshipFeature);

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
        public void ShouldShowFindApprenticeshipLink(int apprenticeshipCount, int traineeshipCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(apprenticeshipCount);
            AddTraineeships(traineeshipCount);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

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
                _apprenticeships, _traineeships, _traineeshipFeature);

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
        public void ShouldShowSuccessfulCount(int successfulCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(successfulCount, ApplicationStatuses.Successful);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("successful-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(successfulCount));
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
        public void ShouldShowSubmittedCount(int submittingCount, int submittedCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(submittingCount, ApplicationStatuses.Submitting);
            AddApprenticeships(submittedCount, ApplicationStatuses.Submitted);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("submitted-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(
                    submittingCount + submittedCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowUnsuccessfulCount(int unsuccessfulCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(unsuccessfulCount, ApplicationStatuses.Unsuccessful);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("unsuccessful-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(unsuccessfulCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowDraftCount(int draftCount, bool shouldShow)
        {
            // Arrange.
            AddApprenticeships(draftCount, ApplicationStatuses.Draft);

            var myApplications = new MyApplicationsViewModel(
                _apprenticeships, _traineeships, _traineeshipFeature);

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("draft-applications-count");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
                elem.InnerHtml.Should().Be(Convert.ToString(draftCount));
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        #region Helpers

        private void SetUpEmptyViewModels()
        {
            _apprenticeships = new List<MyApprenticeshipApplicationViewModel>();
            _traineeships = new List<MyTraineeshipApplicationViewModel>();
            _traineeshipFeature = new TraineeshipFeatureViewModel();
        }

        private void AddApprenticeships(int count, ApplicationStatuses applicationStatus = ApplicationStatuses.Unknown)
        {
            for (var i = 0; i < count; i++)
            {
                _apprenticeships.Add(new MyApprenticeshipApplicationViewModel
                {
                    ApplicationStatus = applicationStatus
                });
            }
        }

        private void AddTraineeships(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _traineeships.Add(new MyTraineeshipApplicationViewModel());
            }
        }

        #endregion
    }
}
