namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using System.Linq;
    using Configuration;

    [TestFixture]
    public class SendGridConfigurationTests
    {
        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetUserNameConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.UserName);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.Password);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = SendGridConfiguration.Instance.Templates;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count() > 1);
        }

        [TestCase(0), Category("Integration"), Category("SmokeTests")]
        [TestCase(1), Category("Integration"), Category("SmokeTests")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = SendGridConfiguration.Instance.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Id);
        }

        [Ignore("Email is per environment so this needs reviewed, also not allowd to send from noreply")]
        [TestCase(0, "donotreply@local.apprenticeships.gov.uk"), Category("Integration")]
        public void ShouldGetFromEmailConfiguration(int index, string expectedFromEmail)
        {
            var template = SendGridConfiguration.Instance.Templates.ElementAt(index);

            Assert.AreEqual(expectedFromEmail, template.FromEmail);
        }
    }
}
