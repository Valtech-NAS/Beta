using System;
using System.Reflection;
using OpenQA.Selenium;
using SpecBind.BrowserSupport;
using SpecBind.Selenium;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    public class BindingUtils
    {
        internal static IWebDriver Driver(IBrowser browser)
        {
            var selenuimBrowser = browser as SeleniumBrowser;
            if (selenuimBrowser == null)
            {
                var reusableBrowser = browser as ReusableBrowser;
                var browserField = typeof(ReusableBrowser).GetField("browser", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                if (reusableBrowser != null && browserField != null)
                {
                    var browserValue = browserField.GetValue(reusableBrowser);
                    selenuimBrowser = browserValue as SeleniumBrowser;
                }
            }
            if (selenuimBrowser == null)
                throw new ArgumentException(string.Format("Browser is not of a compatible type. Expected either SeleniumBrowser or ReusableBrowser. Actual {0}", browser.GetType()));
            var field = typeof(SeleniumBrowser).GetField("driver", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var value = field.GetValue(selenuimBrowser);
            return (value as System.Lazy<IWebDriver>).Value;
        } 
    }
}