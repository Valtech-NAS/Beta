namespace SFA.Apprenticeships.Common.AppSettings
{
    using System.Configuration;

    /// <summary>
    /// Abstraction for getting configuration values from either the Azure role environment or app.config / web.config 
    /// </summary>
    public class AppConfiguration
    {
        public static string GetValue(string key)
        {
            string configurationValue;

            try
            {
              configurationValue = ConfigurationManager.AppSettings[key];
            }
            catch
            {
                //return a null value in case of an exception so that later we can set default value against nul values. 
                configurationValue = null;
            }

            return configurationValue;
        }
    }
}
