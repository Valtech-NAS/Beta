using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Services.Common.Configuration;

namespace SFA.Apprenticeships.Services.Common.Tests.ConfigurationManger
{
    /// <summary>
    /// This is a test class for ConfigurationUtilities and is intended
    /// to contain all ConfigurationUtilitiesTest Unit Tests
    /// Note that this test project does not copy configuration files in from
    /// any other project. It has it's own configuration files used to test 
    /// very specific configuration scenarios
    /// </summary>
    [TestFixture]
    public class ConfigurationUtilitiesTest
    {
        #region GetConnectionString tests
        /// <summary>
        /// A test for GetConnectionString
        /// </summary>
        [TestCase, Description("Tests that the GetConnectionString method throws an ArgumentNullException if the key supplied is null.")]
        public void GetConnectionString_NullKey_Test()
        {
            var target = new ConfigurationUtilities();
            string key = null;
            Action result = () => target.GetConnectionString(key);

            result.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// A test for GetConnectionString
        /// </summary>
        [TestCase]
        public void GetConnectionString_AllValid_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "ApplicationExceptions";

            var result = target.GetConnectionString(key);

            const string expectedConnectionString = "TestConnectionString";
            const string expectedConnectionProvider = "System.Data.SqlClient";

            result.Should().NotBeNull();
            result.ConnectionString.Should().Be(expectedConnectionString);
            result.ProviderName.Should().Be(expectedConnectionProvider);
        }

        /// <summary>
        /// A test for GetConnectionString
        /// </summary>
        [TestCase]
        public void GetConnectionString_Missing_Test()
        {
            var target = new ConfigurationUtilities();
            Action test = () => target.GetConnectionString("missingkey");

            test.ShouldThrow<ApplicationException>();
        }

        #endregion

        #region GetAppSetting tests

        /// <summary>
        /// A test for GetAppSetting
        /// </summary>
        [TestCase]
        public void GetAppSetting_NullKey_Test()
        {
            var target = new ConfigurationUtilities();
            string key = null;
            Action result = () => target.GetAppSetting(key);

            result.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// A test for GetAppSetting
        /// </summary>
        [TestCase]
        public void GetAppSetting_AllValid_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "TestValue";

            var result = target.GetAppSetting(key);

            result.Should().NotBeNull();
            result.Should().Be("TestResult");
        }

        /// <summary>
        /// A test for GetAppSetting
        /// </summary>
        [TestCase]
        public void GetAppSetting_Missing_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "MissingKey";

            Action test = () => target.GetAppSetting(key);

            test.ShouldThrow<ArgumentException>();
        }

        #endregion

        #region TryGetAppSetting tests

        /// <summary>
        /// A test for TryGetAppSetting
        /// </summary>
        [TestCase]
        public void TryGetAppSetting_NullKey_Test()
        {
            var target = new ConfigurationUtilities();
            string key = null;

            Action result = () => target.TryGetAppSetting(key);

            result.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// A test for TryGetAppSetting
        /// </summary>
        [TestCase]
        public void TryGetAppSetting_AllValid_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "TestValue";

            var result = target.TryGetAppSetting(key);

            result.Should().NotBeNull();
            result.Should().Be("TestResult");
        }

        #endregion

        #region GetAppSetting<t> tests

        /// <summary>
        /// Gets the app setting test helper.
        /// </summary>
        /// <typeparam name="T">type parameter</typeparam>
        /// <param name="key">The input key.</param>
        /// <param name="target">The target.</param>
        /// <returns>The value located by the supplied key</returns>
        public T GetAppSettingTestHelper<T>(string key, ConfigurationUtilities target)
        {
            var returnType = default(T);
            var result = target.GetAppSetting<T>(returnType, key);

            return result;
        }

        /// <summary>
        /// A test for GetAppSetting
        /// </summary>
        [TestCase]
        [SetCulture("en-GB")]
        public void GetAppSetting_Generic_AllValid_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "TestDate";
            var result = target.GetAppSetting<DateTime>(key);
            result.Should().Be(new DateTime(2014, 12, 31));
        }

        /// <summary>
        /// A test for GetAppSetting
        /// </summary>
        [TestCase]
        public void GetAppSetting_Generic_Missing_Test()
        {
            var target = new ConfigurationUtilities();
            const string key = "Missingvalue";

            Action test = () => target.GetAppSetting<DateTime>(key);

            test.ShouldThrow<ArgumentException>();
        }

        /// <summary>
        /// Checks the get app setting throws exception on no key.
        /// </summary>
        [TestCase]
        public void CheckGetAppSettingThrowsExceptionOnNoKey1()
        {
            var target = new ConfigurationUtilities();
            Action result = () => target.GetAppSetting<DateTime>(string.Empty);

            result.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Checks the get app setting throws exception on no key.
        /// </summary>
        [TestCase]
        public void CheckGetAppSettingThrowsExceptionOnNoKey2()
        {
            var target = new ConfigurationUtilities();
           Action result = () => target.GetAppSetting<DateTime>(DateTime.Today, string.Empty);

            result.ShouldThrow<ArgumentNullException>();
        }

        /// <summary>
        /// Checks the get app setting throws exception on no key.
        /// </summary>
        [TestCase]
        public void CheckGetAppSettingThrowsExceptionInvalidKey()
        {
            var target = new ConfigurationUtilities();
            Action result = () => target.GetAppSetting("test");

            result.ShouldThrow<ArgumentException>();
        }
        #endregion
    }
}
