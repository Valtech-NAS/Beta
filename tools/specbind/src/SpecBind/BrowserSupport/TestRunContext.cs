// <copyright file="WebDriverContext.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.BrowserSupport
{
    using TechTalk.SpecFlow;

    /// <summary>
    /// Spec Flow Context that survives the entire test run
    /// </summary>
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

        public void Remove<T>(T data)
        {
            base.Remove(this.GetDefaultKey<T>());
        }

        private string GetDefaultKey<T>()
        {
            return typeof(T).FullName;
        }
    }
}