// <copyright file="ReusableBrowser.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.BrowserSupport
{
    using System;
    using System.Collections.Generic;

    using SpecBind.Pages;

    /// <summary>
    /// Wraps an IBrowser instance allowing it to survive the disposal of the SpecFlow IObjectContainer
    /// </summary>
    public class ReusableBrowser : IBrowser
    {
        private readonly IBrowser browser;

        /// <summary>
        /// Constructs a Reusable Browser wrapper for a concrete IBrowser instance
        /// </summary>
        /// <param name="browser">The concrete IBrowser instance</param>
        public ReusableBrowser(IBrowser browser)
        {
            this.browser = browser;
        }

        public Type BasePageType
        {
            get
            {
                return browser.BasePageType;
            }
        }

        public string Url
        {
            get
            {
                return browser.Url;
            }
        }

        public void Close()
        {
            browser.Close();

// ReSharper disable SuspiciousTypeConversion.Global
			var dispoable = browser as IDisposable;
// ReSharper restore SuspiciousTypeConversion.Global
			if (dispoable != null)
			{
				dispoable.Dispose();
			}
        }

        public void DismissAlert(AlertBoxAction action, string text)
        {
            browser.DismissAlert(action, text);
        }

        public void EnsureOnPage(IPage page)
        {
            browser.EnsureOnPage(page);
        }

        public string GetUriForPageType(Type pageType)
        {
            return browser.GetUriForPageType(pageType);
        }

        public void GoTo(Uri url)
        {
            browser.GoTo(url);
        }

        public IPage GoToPage(Type pageType, IDictionary<string, string> parameters)
        {
            return browser.GoToPage(pageType, parameters);
        }

        public IPage Page(Type pageType)
        {
            return browser.Page(pageType);
        }

        public string TakeScreenshot(string imageFolder, string fileNameBase)
        {
            return browser.TakeScreenshot(imageFolder, fileNameBase);
        }

        public string SaveHtml(string destinationFolder, string fileNameBase)
        {
            return browser.SaveHtml(destinationFolder, fileNameBase);
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return browser.ExecuteScript(script, args);
        }

        public void DeleteAllCookies()
        {
            browser.DeleteAllCookies();
        }
    }
}