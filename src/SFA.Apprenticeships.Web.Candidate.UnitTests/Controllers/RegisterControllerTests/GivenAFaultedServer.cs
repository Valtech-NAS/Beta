namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Controllers.RegisterControllerTests
{
    using System.Web;
    using System.Web.Mvc;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Web.Candidate.Controllers;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using SFA.Apprenticeships.Web.Candidate.UnitTests.Mocks;
    using SFA.Apprenticeships.Web.Common.IoC;
    using StructureMap;

    [TestFixture, Ignore("Rethink the way the controllers are created")]
    public class GivenAFaultedServer
    {
        [TestCase]
        public void WhenICheckAUsername_ThenIReceiveAJsonWithHasError()
        {
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            var username = "username";

            // We should change this to an injection of these objects ---
            RegisterDependencies();

            candidateServiceProvider.Setup(csp => csp.IsUsernameAvailable(username))
                .Returns(new Constants.ViewModels.UserNameAvailability() { HasError = true });

            var registerController = new RegisterController(candidateServiceProvider.Object, null, null, null,
                null, null);

            var result = registerController.CheckUsername(username);
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
        }

        private void RegisterDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<WebCommonRegistry>();
                x.For<HttpContextBase>().Use(GetFakeHttpContext());
            });
            WebCommonRegistry.Configure(ObjectFactory.Container);
        }

        private HttpContextBase GetFakeHttpContext()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();

            HttpCookieCollection cookieCollection = new HttpCookieCollection();
            request.Setup(r => r.Cookies).Returns(cookieCollection);
            response.Setup(r => r.Cookies).Returns(cookieCollection);

            var context = new Mock<HttpContextBase>();
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);


            return context.Object;
        }
    }
}
