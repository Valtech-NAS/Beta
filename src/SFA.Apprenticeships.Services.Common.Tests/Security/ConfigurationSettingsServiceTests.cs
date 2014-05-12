using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Configuration;

namespace SFA.Apprenticeships.Services.Common.Tests.Security
{
    [TestFixture]
    public class ConfigurationSettingsServiceTests
    {
        [TestCase]
        public void CtorCorrectlyLoadsXmlFile()
        {
            Action test = () => new ConfigurationSettingsService(GetSettings(), ConfigurationSettingsType.Xml);

            test.ShouldNotThrow();
        }

        [TestCase]
        public void CtorThrowsExceptionWhenConfigIsInvalid()
        {
            Action test = () => new ConfigurationSettingsService("<?xml version=\"1.0\" encoding=\"utf-8\" ?><debug>", ConfigurationSettingsType.Xml);

            test.ShouldThrow<XmlException>();
        }

        [TestCase]
        public void GetSettingThrowsExceptionWhenNoKeyMatch()
        {
            var service = new ConfigurationSettingsService(GetSettings(), ConfigurationSettingsType.Xml);

            Action test = () => service.GetSetting("Test.Invalid");

            test.ShouldThrow<KeyNotFoundException>();
        }

        [TestCase]
        public void GetSettingByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationSettingsService(GetSettings(), ConfigurationSettingsType.Xml);

            service.GetSetting("ReferenceDataService.Username").Should().Be("username");
        }

        [TestCase]
        public void GetSettingByTypeByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationSettingsService(GetSettings(), ConfigurationSettingsType.Xml);

            service.GetSetting<int>("Test.Value").Should().Be(99);
        }

        private string GetSettings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (
                var stream =
                    assembly.GetManifestResourceStream(
                        "SFA.Apprenticeships.Services.Common.Tests.App_Data.test.settings.config"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
