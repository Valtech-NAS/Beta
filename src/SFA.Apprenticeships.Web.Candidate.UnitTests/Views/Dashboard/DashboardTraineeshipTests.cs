namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using Candidate.ViewModels.Applications;
    using Candidate.Views.Account;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardTraineeshipTests : ViewUnitTest
    {

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowTraineeshipPrompt(bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsPrompt = shouldShow
                }).Build();

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


        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowFindTraineeshipLink(bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = shouldShow
                }).Build();

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

        [TestCase(0,false)]
        [TestCase(2, true)]
        public void ShowTraineeships(int traineeshipCount, bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetTraineeships(traineeshipCount)).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var traineeshipsCount = view.GetElementbyId("traineeship-applications-count");
            var traineeshipsTable = view.GetElementbyId("dashTraineeships");

            if (shouldShow)
            {
                traineeshipsCount.Should().NotBeNull();
                traineeshipsCount.InnerHtml.Should().Be(Convert.ToString(traineeshipCount));
                traineeshipsTable.Should().NotBeNull();
            }
            else
            {
                traineeshipsCount.Should().BeNull();
                traineeshipsTable.Should().BeNull();
            }
        }
    }
}