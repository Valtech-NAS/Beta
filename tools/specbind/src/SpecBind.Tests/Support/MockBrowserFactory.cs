﻿// <copyright file="MockBrowserFactory.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.Tests.Support
{
    using Moq;

    using SpecBind.Actions;
    using SpecBind.BrowserSupport;
    using SpecBind.Configuration;

    /// <summary>
    /// A Mock Browser Factory used for testing.
    /// </summary>
    public class MockBrowserFactory : BrowserFactory
    {
        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <value>The browser.</value>
        public Mock<IBrowser> BrowserMock { get; private set; }

        /// <summary>
        /// Creates the browser.
        /// </summary>
        /// <param name="browserType">Type of the browser.</param>
        /// <param name="browserFactoryConfiguration">The browser factory configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A browser object.</returns>
        protected override IBrowser CreateBrowser(BrowserType browserType, BrowserFactoryConfigurationElement browserFactoryConfiguration, ILogger logger)
        {
            BrowserMock = new Mock<IBrowser>();
            return BrowserMock.Object;
        }
    }
}