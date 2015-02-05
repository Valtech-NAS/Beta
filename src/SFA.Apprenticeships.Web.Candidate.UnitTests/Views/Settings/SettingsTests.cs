namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using System;
    using System.Web.Routing;
    using Candidate.ViewModels;
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Locations;
    using Candidate.Views.Account;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class SettingsTests
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShowFindTraineeshipLink(bool shouldShow)
        {
            var viewModel = new SettingsViewModel
            {
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = shouldShow,ShowTraineeshipsPrompt = false
                }
            };

            var view = new Settings {ViewData = {Model = viewModel}};
            var result = view.RenderAsHtml(viewModel);

            var findTraineeshipLink = result.GetElementbyId("find-traineeship-link");

            if (shouldShow)
            {
                findTraineeshipLink.Should().NotBeNull();
            }
            else
            {
                findTraineeshipLink.Should().BeNull();
            }
        }
    }
}