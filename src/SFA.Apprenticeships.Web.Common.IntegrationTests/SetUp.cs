namespace SFA.Apprenticeships.Web.Common.IntegrationTests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using FluentAutomation;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;

    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public virtual void BeforeAllTests()
        {
            SeleniumWebDriver.Bootstrap(SeleniumWebDriver.Browser.Firefox);
            FluentSession.EnableStickySession();

            //See for more settings: http://fluent.stirno.com/docs/#settings
            FluentSettings.Current.WaitTimeout = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public virtual void AfterAllTests()
        {
            var driverProcesses =
                Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver" || pr.ProcessName == "firefox");

            foreach (var process in driverProcesses)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception ex)
                {
                    Console.Write(
                        "Error killing Process: {0}\nError: {1}\nStacktrace: {2}", 
                        process.ProcessName,
                        ex.Message, 
                        ex.StackTrace);
                }
            }
        }
    }
}