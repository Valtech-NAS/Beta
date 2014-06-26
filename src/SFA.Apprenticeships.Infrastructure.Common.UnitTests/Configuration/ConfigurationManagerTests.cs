namespace SFA.Apprenticeships.Infrastructure.Common.UnitTests.Configuration
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using Common.Configuration;

    [TestFixture]
    public class ConfigurationManagerTests
    {
        [Test]
        public void CtorCorrectlyLoadsConfigFile()
        {
            Action test = () => new ConfigurationManager();

            test.ShouldNotThrow();
        }

        [Test]
        public void GetSettingThrowsExceptionWhenNoKeyMatch()
        {
            var service = new ConfigurationManager();

            Action test = () => service.GetAppSetting("Test.Invalid");

            test.ShouldThrow<KeyNotFoundException>();
        }

        [Test]
        public void GetSettingByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager();

            service.GetAppSetting("Test.StringValue").Should().Be("validstring");
        }

        [Test]
        public void GetSettingByTypeByKeyReturnsExpectedValue()
        {
            var service = new ConfigurationManager();

            service.GetAppSetting<int>("Test.IntValue").Should().Be(99);
        }

        [Test]
        [SetCulture("en-GB")]
        public void GetAppSettingReturnsStronglyTypedValue()
        {
            var target = new ConfigurationManager();
            const string key = "Test.DateValue";
            var result = target.GetAppSetting<DateTime>(key);
            result.Should().Be(new DateTime(2014, 12, 31));
        }

        [Test]
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
