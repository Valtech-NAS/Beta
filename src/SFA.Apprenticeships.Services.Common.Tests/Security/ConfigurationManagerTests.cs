using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.ActiveDirectory;
using SFA.Apprenticeships.Services.Common.Configuration;

namespace SFA.Apprenticeships.Services.Common.Tests.Security
{
    [TestFixture]
    public class ConfigurationManagerTests
    {
        [TestCase]
        public void CtorCorrectlyLoadsXmlFile()
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
        [Ignore("Need way of specifying file location")]
        public void GetConfigSectionForAd()
        {
            var test = ActiveDirectoryConfigurationSection.ConfigurationSectionDetails;
           
            test.DistinguishedName.Should().Be("distinguishedname");
            test.Server.Should().Be("server");
            test.Username.Should().Be("username");
            test.Password.Should().Be("password");
        }


        private string GetSettings()
        {
           return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "\\App_Data\\test.settings.config");
        }
    }
}
