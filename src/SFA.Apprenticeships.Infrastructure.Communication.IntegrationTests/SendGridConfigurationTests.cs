namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using System.Linq;
    using Configuration;

    [TestFixture]
    public class SendGridConfigurationTests
    {
        [Test, Category("Integration")]
        public void ShouldGetUserNameConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.UserName);
        }

        [Test, Category("Integration")]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.Password);
        }

        [Test, Category("Integration")]
        public void ShouldGetMultipleTemplates()
        {
            var templates = SendGridConfiguration.Instance.Templates;

            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.Count() > 1);
        }

        [TestCase(0), Category("Integration")]
        [TestCase(1), Category("Integration")]
        public void ShouldGetMultipleTemplateConfiguration(int index)
        {
            var template = SendGridConfiguration.Instance.Templates.ElementAt(index);

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Id);
        }

        [TestCase(0, "donotreply@example.com"), Category("Integration")]
        public void ShouldGetFromEmailConfiguration(int index, string expectedFromEmail)
        {
            var template = SendGridConfiguration.Instance.Templates.ElementAt(index);

            Assert.AreEqual(expectedFromEmail, template.FromEmail);
        }
    }
}
