namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests
{
    using System;
    using Application.Interfaces.Messaging;
    using Common.IoC;
    using Domain.Interfaces.Messaging;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SubmitApplicationRequestTest
    {
        private IMessageBus _bus;

        [SetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
            });

            _bus = ObjectFactory.GetInstance<IMessageBus>();
        }

        [Test]
        public void ShouldSendSubmitApplicationRequest()
        {
            var message = new SubmitApplicationRequest
            {
                ApplicationId = Guid.NewGuid().ToString()
            };

            _bus.PublishMessage(message);
        }
    }
}