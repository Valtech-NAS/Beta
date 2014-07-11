using System.Collections.Generic;

namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using StructureMap;
    using Common.IoC;
    using IoC;
    using Application.Interfaces.Messaging;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            // Arrange / Act.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            // Assert.
            Assert.IsNotNull(dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(dispatcher);
        }

        [Test]
        public void ShoudSendEmail()
        {
            // Arrange.
            var dispatcher = ObjectFactory.GetInstance<IEmailDispatcher>();

            var request = new EmailRequest
            {
                Subject = "Hello, World",
                FromEmail = "from@example.com",
                ToEmail = "to@example.com",
                Tokens = new KeyValuePair<string, string>[]
                {
                }
            };

            // Act.
            dispatcher.SendEmail(request);

            // Assert.
            Assert.Inconclusive();
        }
    }
}
