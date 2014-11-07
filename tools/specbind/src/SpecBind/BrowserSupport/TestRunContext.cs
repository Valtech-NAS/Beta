// <copyright file="WebDriverContext.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.BrowserSupport
{
    using TechTalk.SpecFlow;

    public class TestRunContext : SpecFlowContext
    {
        private static TestRunContext current;

        public static TestRunContext Current
        {
            get
            {
                if (current == null)
                {
                    current = new TestRunContext();
                }
                return current;
            }
        }
    }
}