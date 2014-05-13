using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.ActiveDirectory;
using SFA.Apprenticeships.Common.Configuration;

namespace SFA.Apprenticeships.Services.Common.Tests.ConfigurationManger
{
    [TestFixture]
    public class ConfigurationManagerTests
    {
        [TestCase]
        public void CtorCorrectlyLoadsConfigFile()
        {
            Action test = () => new ConfigurationManager(GetSettings());

            test.ShouldNotThrow();
        }

        [TestCase]
        public void GetSettingThrowsExceptionWhenNoKeyMatch()
        {
            var service = new ConfigurationManager(GetSettings());

            Action test = () => service.GetAppSetting("Test.Invalid");

            test.ShouldThrow<KeyNotFoundException>();
        }

        [TestCase]
        public void GetSettingByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager(GetSettings());

            service.GetAppSetting("ReferenceDataService.Username").Should().Be("username");
        }

        [TestCase]
        public void GetSettingByTypeByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager(GetSettings());

            service.GetAppSetting<int>("Test.Value").Should().Be(99);
        }

        [TestCase]
        public void GetConfigSectionForAd()
        {
            var test = ActiveDirectoryConfigurationSection.ConfigurationSectionDetails;
           
            test.DistinguishedName.Should().Be("distinguishedname");
            test.Server.Should().Be("server");
            test.Username.Should().Be("username");
            test.Password.Should().Be("password");
        }

        [TestCase]
        [SetCulture("en-GB")]
        public void GetAppSettingReturnsStronglyTypedValue()
        {
            var target = new ConfigurationManager(GetSettings());
            const string key = "Test.Date";
            var result = target.GetAppSetting<DateTime>(key);
            result.Should().Be(new DateTime(2014, 12, 31));
        }

        [TestCase]
        [SetCulture("en-GB")]
        public void GetAppSettingReturnsStronglyTypedDefaultValue()
        {
            var target = new ConfigurationManager(GetSettings());
            const string key = "Test.Date2";
            var result = target.GetAppSetting<DateTime>(new DateTime(2014,1,1), key);
            result.Should().Be(new DateTime(2014, 1, 1));
        }

        private string GetSettings()
        {
            return System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting];
        }
    }
}
