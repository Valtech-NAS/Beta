namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Controllers.RegisterControllerTests
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Candidate.Controllers;
    using Candidate.Providers;
    using Common.IoC;
    using Constants.ViewModels;
    using Infrastructure.Common.IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;
    using FluentAssertions;

    [TestFixture]
    public class GivenAFaultedServer
    {
        //[TestCase]
        //public async Task WhenICheckAUsername_ThenIReceiveAJsonWithHasError()
        //{
        //    // Arrange
        //    var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
        //    const string username = "username";

        //    RegisterDependencies(null);

        //    candidateServiceProvider.Setup(csp => csp.IsUsernameAvailable(username))
        //        .Returns(new UserNameAvailability {HasError = true});

        //    var registerController = new RegisterController(candidateServiceProvider.Object, null, null, null,
        //        null, null);

        //    // Act
        //    var result = await registerController.CheckUsername(username);

        //    //Assert
        //    AssertTheViewModelHasError(result);
        //}

        private static void AssertTheViewModelHasError(ActionResult result)
        {
            var jsonResult = result as JsonResult;
            jsonResult.Should().NotBeNull();
// ReSharper disable once PossibleNullReferenceException
            var data = jsonResult.Data as UserNameAvailability;
            data.Should().NotBeNull();
// ReSharper disable once PossibleNullReferenceException
            data.HasError.Should().BeTrue();
        }

        private static void RegisterDependencies(HttpContextBase contextBase)
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<WebCommonRegistry>();
                x.For<HttpContextBase>().Use( contextBase ?? GetBasicFakeHttpContext());
            });

            WebCommonRegistry.Configure(ObjectFactory.Container);
#pragma warning restore 0618
        }

        private static HttpContextBase GetBasicFakeHttpContext()
        {
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();

            var cookieCollection = new HttpCookieCollection();
            request.Setup(r => r.Cookies).Returns(cookieCollection);
            response.Setup(r => r.Cookies).Returns(cookieCollection);

            var context = new Mock<HttpContextBase>();
            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);


            return context.Object;
        }
    }
}