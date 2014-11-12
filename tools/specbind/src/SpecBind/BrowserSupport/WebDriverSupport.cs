// <copyright file="WebDriverSupport.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.BrowserSupport
{
	using System;
	using System.Diagnostics;
	using System.IO;

	using BoDi;

	using SpecBind.ActionPipeline;
	using SpecBind.Actions;
	using SpecBind.Helpers;
	using SpecBind.Pages;

	using TechTalk.SpecFlow;
	using TechTalk.SpecFlow.Tracing;

    /// <summary>
	/// A hooks support class for the web driver.
	/// </summary>
	[Binding]
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public class WebDriverSupport
	{
        private readonly IObjectContainer objectContainer;

        private readonly bool ensureCleanSession;
        private readonly bool reuseBrowser;

        /// <summary>
		/// Initializes a new instance of the <see cref="WebDriverSupport" /> class.
		/// </summary>
		/// <param name="objectContainer">The object container.</param>
		public WebDriverSupport(IObjectContainer objectContainer)
		{
			this.objectContainer = objectContainer;
            var configSection = SettingHelper.GetConfigurationSection();
            var browserFactoryConfigurationElement = configSection.BrowserFactory;
            ensureCleanSession = browserFactoryConfigurationElement.EnsureCleanSession;
		    reuseBrowser = browserFactoryConfigurationElement.ReuseBrowser;
		}

        /// <summary>
        /// Checks the browser factory for any necessary drivers.
        /// </summary>
        [BeforeTestRun]
        public static void CheckForDriver()
        {
            var factory = BrowserFactory.GetBrowserFactory(new NullLogger());
            factory.ValidateDriverSetup();
        }

		/// <summary>
		/// Initializes the page mapper at the start of the test run.
		/// </summary>
		[BeforeScenario]
		public void InitializeDriver()
		{
            this.objectContainer.RegisterTypeAs<ProxyLogger, ILogger>();
		    var logger = this.objectContainer.Resolve<ILogger>();

            IBrowser browser;
		    if (this.reuseBrowser)
		    {
                if (!TestRunContext.Current.TryGetValue(out browser))
                {
                    var factory = BrowserFactory.GetBrowserFactory(logger);
                    browser = new ReusableBrowser(factory.GetBrowser());
                    TestRunContext.Current.Set(browser);
                }
		    }
		    else
		    {
                var factory = BrowserFactory.GetBrowserFactory(logger);
                browser = factory.GetBrowser();
		    }
            this.objectContainer.RegisterInstanceAs(browser);

		    this.objectContainer.RegisterInstanceAs<ISettingHelper>(new WrappedSettingHelper());

			var mapper = new PageMapper();
			mapper.Initialize(browser.BasePageType);
			this.objectContainer.RegisterInstanceAs<IPageMapper>(mapper);

			this.objectContainer.RegisterInstanceAs<IScenarioContextHelper>(new ScenarioContextHelper());
			this.objectContainer.RegisterInstanceAs(TokenManager.Current);

		    var repository = new ActionRepository(this.objectContainer);
			this.objectContainer.RegisterInstanceAs<IActionRepository>(repository);
			this.objectContainer.RegisterTypeAs<ActionPipelineService, IActionPipelineService>();
            
            // Initialize the repository
            repository.Initialize();
		}

		/// <summary>
		/// Tears down the web driver.
		/// </summary>
		[AfterScenario]
		public void TearDownWebDriverAfterScenario()
		{
            var browser = this.objectContainer.Resolve<IBrowser>();

            // Check for an error and capture a screenshot
            this.CheckForScreenshot(browser);

            if (this.reuseBrowser)
		    {
		        if (ScenarioHasErrors())
		        {
                    //In the event of an error, close and dispose of the browser instance as the test could have left it in an invalid state
                    TearDownReusableBrowser(browser);
		        }
                else if (this.ensureCleanSession)
		            browser.DeleteAllCookies();
		    }
		    else
		    {
		        // Only need to call close as the disposal of the IObjectContainer will call dispose on the browser instance if it implements IDisposable
		        browser.Close();
		    }
		}

        private static void TearDownReusableBrowser(IBrowser browser)
        {
            try
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
            finally
            {
                TestRunContext.Current.Remove(browser);
            }
        }

        /// <summary>
        /// Tears down the web driver.
        /// </summary>
        [AfterTestRun]
        public static void TearDownWebDriverAfterTestRun()
        {
            IBrowser browser;
            if (TestRunContext.Current.TryGetValue(out browser))
            {
                browser.Close();
            }
        }

        /// <summary>
        /// Checks for screenshot.
        /// </summary>
        /// <param name="browser">The browser.</param>
        private void CheckForScreenshot(IBrowser browser)
        {
            var scenarioHelper = this.objectContainer.Resolve<IScenarioContextHelper>();
            if (!ScenarioHasErrors(scenarioHelper)) return;
            
            var fileName = scenarioHelper.GetStepFileName();
            var basePath = Directory.GetCurrentDirectory();
            var fullPath = browser.TakeScreenshot(basePath, fileName);
            browser.SaveHtml(basePath, fileName);

            var traceListener = this.objectContainer.Resolve<ITraceListener>();
            if (fullPath != null && traceListener != null)
            {
                traceListener.WriteTestOutput("Created Error Screenshot: {0}", fullPath);       
            }
        }

        private bool ScenarioHasErrors()
        {
            var scenarioHelper = this.objectContainer.Resolve<IScenarioContextHelper>();
            return ScenarioHasErrors(scenarioHelper);
        }

        private static bool ScenarioHasErrors(IScenarioContextHelper scenarioHelper)
        {
            return scenarioHelper.GetError() != null;
        }
	}
}