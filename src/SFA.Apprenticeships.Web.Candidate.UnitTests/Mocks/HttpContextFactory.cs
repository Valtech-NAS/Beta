namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mocks
{
    using System;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;

    /// <summary>
    /// Creates a MockHttpContextbase for use in the session larder manager
    /// </summary>
    public static class HttpContextFactory
    {
        /// <summary>
        /// Sets the fake authenticated controller context.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public static void SetFakeAuthenticatedControllerContext(this Controller controller)
        {
            var httpContext = FakeAuthenticatedHttpContext(true);
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
        }

        /// <summary>
        /// Sets the fake authenticated controller context.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public static void SetFakeUnauthenticatedControllerContext(this Controller controller)
        {
            var httpContext = FakeAuthenticatedHttpContext(false);
            controller.ControllerContext = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
        }

        /// <summary>
        /// Creates a fake authenticated HTTP context.
        /// </summary>
        /// <param name="isAuthenticated">if set to <c>true</c> [is authenticated].</param>
        /// <returns>
        /// The fake HttpContextBase with an authenticated user
        /// </returns>
        private static HttpContextBase FakeAuthenticatedHttpContext(bool isAuthenticated)
        {
            var context = new Mock<HttpContextBase>();

            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var uri = new Uri("http://MyPortfolio");
            var identity = new Mock<IIdentity>();

            NameValueCollection queryString = new NameValueCollection();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(isAuthenticated);
            request.Setup(x => x.UrlReferrer).Returns(uri);
            request.Setup(x => x.QueryString).Returns(queryString);

            if (isAuthenticated)
            {
                identity.Setup(id => id.Name).Returns("nunit");
            }

            return context.Object;
        }
    }
}