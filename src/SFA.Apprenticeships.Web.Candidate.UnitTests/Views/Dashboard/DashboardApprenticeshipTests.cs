namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using Candidate.Views.Account;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardApprenticeshipTests : ViewUnitTest
    {
        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(0, 1, false)]
        public void ShouldShowFindApprenticeshipButton(int apprenticeshipCount, int traineeshipCount, bool shouldShow)
        {
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(apprenticeshipCount))
                    .With(DashboardTestsHelper.GetTraineeships(traineeshipCount))
                    .Build();

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
            var myApplications = new MyApplicationsViewModelBuilder().Build();
            myApplications.DeletedVacancyId = deletedVacancyId;
            myApplications.DeletedVacancyTitle = deletedVacancyTitle;

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

        [TestCase(0, 0, false)]
        [TestCase(2, 0, true)]
        [TestCase(2, 3, true)]
        public void ShouldShowFindApprenticeshipLink(int apprenticeshipCount, int traineeshipCount, bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(apprenticeshipCount))
                    .With(DashboardTestsHelper.GetTraineeships(traineeshipCount))
                    .Build();

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

        [TestCase(0, false)]
        [TestCase(2, true)]
        public void ShouldShowSuccessfulCount(int successfulCount, bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(successfulCount,
                    ApplicationStatuses.Successful)).Build();

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
            var apprenticeships = DashboardTestsHelper.GetApprenticeships(submittingCount,
                ApplicationStatuses.Submitting);
            apprenticeships.AddRange(DashboardTestsHelper.GetApprenticeships(submittedCount,
                ApplicationStatuses.Submitted));

            var myApplications = new MyApplicationsViewModelBuilder().With(apprenticeships).Build();

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
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(unsuccessfulCount,
                    ApplicationStatuses.Unsuccessful)).Build();


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
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(draftCount,
                    ApplicationStatuses.Draft)).Build();

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

        public void ShouldShowCandidateSupportMessage()
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1,
                    ApplicationStatuses.Unsuccessful)).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("candidate-support-message");

            elem.Should().NotBeNull();
        }

        [Test]
        public void ShouldNotShowCandidateSupportMessage()
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1,
                    ApplicationStatuses.Successful)).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("candidate-support-message");

            elem.Should().BeNull();
        }

        [Test]
        public void ShouldNotShowCandidateSupportMessageWithExpiredApplications()
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1,
                    ApplicationStatuses.ExpiredOrWithdrawn)).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("candidate-support-message");

            elem.Should().BeNull();
        }
    }
}