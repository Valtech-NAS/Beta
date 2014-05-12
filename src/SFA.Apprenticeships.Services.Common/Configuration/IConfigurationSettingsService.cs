namespace SFA.Apprenticeships.Services.Common.Configuration
{
    public interface IConfigurationSettingsService
    {
        /// <summary>
        /// Gets the configuration setting for the current configuration - ie debug.
        /// The default configuration can be set in the constructor.
        /// </summary>
        string TryGetSetting(string key);
        string GetSetting(string key);
        T GetSetting<T>(string key);
    }
}
