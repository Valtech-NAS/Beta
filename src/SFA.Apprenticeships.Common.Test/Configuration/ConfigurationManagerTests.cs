namespace SFA.Apprenticeships.Common.Tests.Configuration
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration;

    [TestFixture]
    public class ConfigurationManagerTests
    {
        [TestCase]
        public void CtorCorrectlyLoadsConfigFile()
        {
            Action test = () => new ConfigurationManager();

            test.ShouldNotThrow();
        }

        [TestCase]
        public void GetSettingThrowsExceptionWhenNoKeyMatch()
        {
            var service = new ConfigurationManager();

            Action test = () => service.GetAppSetting("Test.Invalid");

            test.ShouldThrow<KeyNotFoundException>();
        }

        [TestCase]
        public void GetSettingByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager();

            service.GetAppSetting("Test.StringValue").Should().Be("validstring");
        }

        [TestCase]
        public void GetSettingByTypeByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager();

            service.GetAppSetting<int>("Test.IntValue").Should().Be(99);
        }

        [TestCase]
        [SetCulture("en-GB")]
        public void GetAppSettingReturnsStronglyTypedValue()
        {
            var target = new ConfigurationManager();
            const string key = "Test.DateValue";
            var result = target.GetAppSetting<DateTime>(key);
            result.Should().Be(new DateTime(2014, 12, 31));
        }

        [TestCase]
        [SetCulture("en-GB")]
        public void GetAppSettingReturnsStronglyTypedDefaultValue()
        {
            var target = new ConfigurationManager();
            const string key = "Test.DateValue.NotPresent";
            var result = target.GetAppSetting<DateTime>(new DateTime(2014,1,1), key);
            result.Should().Be(new DateTime(2014, 1, 1));
        }
    }
}
