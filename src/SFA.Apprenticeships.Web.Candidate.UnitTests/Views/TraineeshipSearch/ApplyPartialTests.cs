namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.TraineeshipSearch
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.TraineeshipSearch;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class ApplyPartialTests : ViewUnitTest
    {
        [Test]
        public void ShouldAllowCandidateToApplyViaEmployerWebsiteWithWellFormedUrl()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                ApplyViaEmployerWebsite = true,
                IsWellFormedVacancyUrl = true,
                VacancyUrl = "http://www.example.com"
            };

            // Act.
            var view = index.RenderAsHtml(vm);

            // Assert.
            var link = view.GetElementbyId("external-employer-website");

            link.Should().NotBeNull();
            link.GetAttributeValue("href", vm.VacancyUrl);

            view.GetElementbyId("external-employer-raw-vacancy-url").Should().BeNull();
        }

        [Test]
        public void ShouldAllowCandidateToApplyForLiveVacancy()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            // Act.
            var httpContext = CreateMockContext(true);
            var view = index.RenderAsHtml(httpContext, vm);

            // Assert.
            var elem = view.GetElementbyId("apply-button");

            elem.Should().NotBeNull();
            elem.InnerHtml.Should().Be("Apply for traineeship");
        }

        [Test]
        public void ShouldAllowCandidateToApplyViaEmployerWebsiteWithBadlyFormedUrl()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                ApplyViaEmployerWebsite = true,
                IsWellFormedVacancyUrl = false,
                VacancyUrl = "Bad URL"
            };

            // Act.
            var view = index.RenderAsHtml(vm);

            // Assert.
            view.GetElementbyId("external-employer-website").Should().BeNull();

            var elem = view.GetElementbyId("external-employer-raw-vacancy-url");

            elem.Should().NotBeNull();
            elem.InnerHtml.ShouldBeEquivalentTo(vm.VacancyUrl);
        }

        [Test]
        public void ShouldAllowCandidateToSignInToApplyForLiveVacancy()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live
            };

            // Act.
            var view = index.RenderAsHtml(vm);

            // Assert.
            var elem = view.GetElementbyId("apply-button");

            elem.Should().NotBeNull();
            elem.InnerHtml.Should().Be("Sign in to apply");
        }

        [Test]
        public void ShouldShowDateAppliedForApplication()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                CandidateApplicationStatus = ApplicationStatuses.Submitted,
                DateApplied = DateTime.Today.AddDays(-1)
            };

            // Act.
            var httpContext = CreateMockContext(true);
            var view = index.RenderAsHtml(httpContext, vm);

            // Assert.
            view.GetElementbyId("date-applied").Should().NotBeNull();
        }

        [Test]
        public void ShouldAllowCandidateToReturnToMyApplicationsForExpiredVacancy()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                CandidateApplicationStatus = ApplicationStatuses.Submitted,
                VacancyStatus = VacancyStatuses.Expired
            };

            // Act.
            var httpContext = CreateMockContext(true);
            var view = index.RenderAsHtml(httpContext, vm);

            // Assert.
            view.GetElementbyId("return-to-my-applications").Should().NotBeNull();
        }

        [Test]
        public void ShouldShowClosingDateForLiveVacancy()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Live,
                ClosingDate = DateTime.Today.AddDays(42)
            };

            // Act.
            var view = index.RenderAsHtml(vm);

            // Assert.
            view.GetElementbyId("vacancy-closing-date").Should().NotBeNull();
        }

        [Test]
        public void ShouldNotShowClosingDateForExpiredVacancy()
        {
            // Arrange.
            var index = new Apply();
            var vm = new VacancyDetailViewModel
            {
                VacancyStatus = VacancyStatuses.Expired,
                ClosingDate = DateTime.Today.AddDays(-42)
            };

            // Act.
            var view = index.RenderAsHtml(vm);

            // Assert.
            view.GetElementbyId("vacancy-closing-date").Should().BeNull();
        }

        private static HttpContextBase CreateMockContext(bool isAuthenticated)
        {
            // Response.
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);

            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // Request.
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);

            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>((virtualPath) => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // HttpContext.
            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);

            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }
    }
}