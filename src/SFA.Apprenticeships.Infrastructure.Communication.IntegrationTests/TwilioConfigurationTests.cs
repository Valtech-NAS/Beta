namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System.Linq;
    using NUnit.Framework;
    using Sms;

    [TestFixture]
    public class TwilioConfigurationTests
    {
        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetAccountSidConfiguration()
        {
            Assert.IsNotNull(TwilioConfiguration.Instance.AccountSid);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetAuthTokenConfiguration()
        {
            Assert.IsNotNull(TwilioConfiguration.Instance.AuthToken);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMobileNumberFromConfiguration()
        {
            Assert.IsNotNull(TwilioConfiguration.Instance.MobileNumberFrom);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = TwilioConfiguration.Instance.TemplateCollection;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count > 1);
        }

        [TestCase(0), Category("Integration"), Category("SmokeTests")]
        [TestCase(1), Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = TwilioConfiguration.Instance.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Message);
        }

        [Test, Category("IntegrationProd"), Category("SmokeTests")]
        public void ShouldGetFromNameConfiguration()
        {
            const int templateIndex = 0;
            const string expectedName = "MessageTypes.SendActivationCode";

            var template = TwilioConfiguration.Instance.Templates.ElementAt(templateIndex);

            Assert.AreEqual(expectedName, template.Name);
        }
    }
}
