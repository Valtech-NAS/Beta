namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Exposing entire configuration file (required by Wcf when using custom factory)
        /// </summary>
        string ConfigurationFilePath { get; }

        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);

        T GetCloudAppSetting<T>(string key);

        string TryGetAppSetting(string key);
    }
}
