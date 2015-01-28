namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System.Web.Routing;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.MyApplications;
    using Candidate.Views.Account;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardTests
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void ShouldShowBigFindApprenticeshipButtonWhenNoApplications()
        {
            var index = new Index();

            var maavm = new MyApprenticeshipApplicationViewModel[] {};
            var mtavm = new MyTraineeshipApplicationViewModel[] {};
            var tfvm = new TraineeshipFeatureViewModel();

            var vm = new MyApplicationsViewModel(maavm, mtavm, tfvm);
            var view = index.RenderAsHtml(vm);

            view.GetElementbyId("find-apprenticeship-button").Should().NotBeNull();
        }
    }
}