namespace SFA.Apprenticeships.Domain.Interfaces.Configuration
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Exposing entire configuration file (required by Wcf when using custom factory)
        /// </summary>
        string ConfigurationFilePath { get; }

        /// <summary>
        /// Gets a string representation of the value located by the supplied key.
        /// If the value is not found an exception is thrown
        /// </summary>
        /// <param name="key">The key to find the required value.</param>
        /// <returns>The value located by the supplied key</returns>
        string GetAppSetting(string key);

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">The type of return</typeparam>
        /// <param name="key">The setting key.</param>
        /// <returns>
        /// The app setting strongly typed
        /// </returns>
        T GetAppSetting<T>(string key);

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">The type of return</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The setting key.</param>
        /// <returns>
        /// The app setting strongly typed
        /// </returns>
        T GetAppSetting<T>(T defaultValue, string key);

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="key">The connection key.</param>
        /// <returns>The specified connection string</returns>
        string GetConnectionString(string key);
    }
}
