namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views
{
    using System.Web.Routing;
    using NUnit.Framework;

    public abstract class ViewUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}