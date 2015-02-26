namespace SFA.Apprenticeships.Common.AppSettings
{
    using System;

    public class BaseAppSettingValues
    {
        public static string ReferenceDataPathWithFile = AppConfiguration.GetValue(BaseAppSettingKeys.ReferenceDataPath) ?? string.Empty;
        public static bool IsReferenceDataFileInDataDirectory = AppConfiguration.GetValue(BaseAppSettingKeys.IsReferenceDataFileInDataDirectory) == null || Convert.ToBoolean(AppConfiguration.GetValue(BaseAppSettingKeys.IsReferenceDataFileInDataDirectory).ToLower());
        public static bool IsDemoModeEnabled = AppConfiguration.GetValue(BaseAppSettingKeys.IsDemoModeEnabled) == null || Convert.ToBoolean(AppConfiguration.GetValue(BaseAppSettingKeys.IsDemoModeEnabled).ToLower());
        public static string EmailDispatcher = AppConfiguration.GetValue(BaseAppSettingKeys.EmailDispatcher) ?? "VoidEmailDispatcher"; //default configured to VoidEmailDispatcher
        public static string NetworkUsername = AppConfiguration.GetValue(BaseAppSettingKeys.NetworkUsername) ?? string.Empty;
        public static string NetworkPassword = AppConfiguration.GetValue(BaseAppSettingKeys.NetworkPassword) ?? string.Empty;
        public static string ToEmailAddress = AppConfiguration.GetValue(BaseAppSettingKeys.ToEmailAddress) ?? string.Empty;
        public static string FromMailAddress = AppConfiguration.GetValue(BaseAppSettingKeys.FromMailAddress) ?? string.Empty;
        public static string FromName = AppConfiguration.GetValue(BaseAppSettingKeys.FromName) ?? string.Empty;
        
    }
}