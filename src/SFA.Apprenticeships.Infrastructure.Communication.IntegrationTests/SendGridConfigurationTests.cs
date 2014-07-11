
namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using System.Linq;
    using Configuration;

    [TestFixture]
    public class SendGridConfigurationTests
    {
        [Test]
        public void ShouldGetUserNameConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.UserName);
        }

        [Test]
        public void ShouldGetPasswordConfiguration()
        {
            Assert.IsNotNull(SendGridConfiguration.Instance.Password);
        }

       [Test]
       public void ShouldGetMultipleTemplates()
       {
           var templates = SendGridConfiguration.Instance.Templates;

           Assert.IsNotNull(templates);
           Assert.IsTrue((templates.Count > 1));
        }

        [Test]
        public void ShouldGetFirstTemplateConfigurationByName()
        {
            var template = SendGridConfiguration.Instance.Templates["Candiate.ActivationCodeEmail"];

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Id);
        }

        [Test]
        public void ShouldGetSecondTemplateConfigurationByIndex()
        {
            var template = SendGridConfiguration.Instance.Templates.ElementAt(1);

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Name);
            Assert.IsNotNull(template.Id);
        }
    }
}
