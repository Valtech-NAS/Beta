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
        [Ignore]
        public void ShowFindTraineeshipLink(bool shouldShow)
        {
            var viewModel = new SettingsViewModel
            {
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = shouldShow,ShowTraineeshipsPrompt = false
                },
                AllowEmailComms = true,
                Address = new AddressViewModel
                {
                    AddressLine1 = "Addres1",
                    AddressLine2 = "Addres2",
                    AddressLine3 = "Addres3",
                    AddressLine4 = "Addres4",
                    GeoPoint = new GeoPointViewModel { Latitude = 1, Longitude = 1},
                    Postcode = "SW12 9SX",
                    Uprn = "Uprn"
                },
                DateOfBirth = new DateViewModel{Day=11,Month=1,Year=1991},
                Firstname = "FirstName",
                Lastname = "LastName",
                PhoneNumber = "PhoneNumber"
            };

            var view = new Settings();
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