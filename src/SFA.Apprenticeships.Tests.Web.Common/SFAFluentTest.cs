namespace SFA.Apprenticeships.Tests.Web.Common
{
    using System;
    using System.Configuration;
    using FluentAutomation;

    public class SfaFluentTest : FluentTest
    {
        public SfaFluentTest()
        {
            SeleniumWebDriver.Bootstrap(
                SeleniumWebDriver.Browser.Chrome,
                SeleniumWebDriver.Browser.Firefox
            );    
        }

        public Uri WebRoot
        {
            get { return new Uri(ConfigurationManager.AppSettings["WebRoot"] ?? "http://localhost/"); }
        }
    }
}
