namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.TraineeshipSearch
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using Candidate.ViewModels.Locations;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.TraineeshipSearch;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DetailsTests : ViewUnitTest
    {
        private const string NoValue = null;
        private const string SomeString = "some string";

        [Test]
        public void ShouldShowSearchReturnUrlLink()
        {
            const string someUrl = "findapprenticeship.service.gov.uk";
            var details = new Details();
            details.ViewBag.SearchReturnUrl = someUrl;

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };
            var view = details.RenderAsHtml(vacancyDetailViewModel);
            view.GetElementbyId("lnk-return-search-results").Should().NotBeNull("Return to search results should be shown.");
            view.GetElementbyId("lnk-return-search-results").Attributes["href"].Value.Should().Be(someUrl);
        }

        [Test]
        public void ShouldNotShowSearchReturnUrlLink()
        {
            var details = new Details();
            
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);
            view.GetElementbyId("lnk-return-search-results").Should().BeNull("Return to search results should not be shown.");
            view.GetElementbyId("lnk-find-traineeship").Should().NotBeNull("Find a traineeship link should be shown.");
        }

        [Test]
        public void ShouldNotSeeAnyInfoIfModelHasError()
        {
            var details = new Details();
           
            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ViewModelMessage = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-info").Should().BeNull("Should not appear any vacancy information");
        }

        [Test]
        public void ShowDistance()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                Distance = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);
            
            view.GetElementbyId("vacancy-distance").InnerText.Should().Be(SomeString + " miles");
        }

        [Test]
        public void DontShowDistanceItThereIsNoDistanceToShow()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                Distance = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-distance").Should().BeNull();
        }
        
        [Test]
        public void ShowFutureProspects()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                FutureProspects = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-future-prospects").InnerText.Should().Be(SomeString);
        }

        [Test]
        public void HideFutureProspects()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel()
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-future-prospects").Should().BeNull();
        }
        
        [Test]
        public void ShowWellFormedEmployerWebSite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsWellFormedEmployerWebsiteUrl = true,
                EmployerWebsite = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-website").Attributes["href"].Value.Should().Be(SomeString, 
                string.Format("The employer website url should be shown and should be {0}", SomeString));
        }

        [Test]
        public void ShowMalformedEmployerWebSite()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                IsWellFormedEmployerWebsiteUrl = false,
                EmployerWebsite = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-employer-website")
                .Should()
                .BeNull("The employer website url should not be shown");
        }
        
        [Test]
        public void ShowOtherInformation()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                OtherInformation = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacany-other-information").InnerText
                .Should()
                .Contain(SomeString, "Other information should be shown if the value is set.");
        }

        [Test]
        public void HideOtherInformation()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                OtherInformation = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("other-information")
                .Should()
                .BeNull("Other information should not be shown if the value is not set.");
        }

        [Test]
        public void ShowExpectedDuration()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ExpectedDuration = SomeString
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-expected-duration").InnerText
                .Should()
                .Be(SomeString);
        }

        [Test]
        public void ShowNotSpecifiedExpectedDuration()
        {
            var details = new Details();

            var vacancyDetailViewModel = new VacancyDetailViewModel
            {
                VacancyAddress = new AddressViewModel(),
                ExpectedDuration = NoValue
            };

            var view = details.RenderAsHtml(vacancyDetailViewModel);

            view.GetElementbyId("vacancy-expected-duration").InnerText
                .Should()
                .Be("Not specified");
        }

        private static HttpContextBase CreateMockContext(bool isAuthenticated)
        {
            // Use Moq for faking context objects as it can setup all members
            // so that by default, calls to the members return a default/null value 
            // instead of a not implemented exception.

            // members were we want specific values returns are setup explicitly.

            // mock the request object
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Loose);
            mockRequest.Setup(m => m.IsLocal).Returns(false);
            mockRequest.Setup(m => m.ApplicationPath).Returns("/");
            mockRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            mockRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            mockRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
            mockRequest.Setup(m => m.IsAuthenticated).Returns(isAuthenticated);

            // mock the response object
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Loose);
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            mockResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            // mock the httpcontext

            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Loose);
            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockHttpContext.Object;
        }
    }
}