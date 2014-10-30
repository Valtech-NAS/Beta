namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IntegrationTests
{
    using System;
    using Application.Interfaces.Messaging;
    using Common.IoC;
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
            });

            _bus = ObjectFactory.GetInstance<IMessageBus>();
#pragma warning restore 0618
        }

        [TearDown]
        public void TearDown()
        {
            // This is needed or the build hangs
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var busToTidy = ObjectFactory.GetInstance<IBus>();
#pragma warning restore 0618

            busToTidy.Advanced.Dispose();
        }

        [Test, Category("Integration")]
        public void ShouldSendSubmitApplicationRequest()
        {
            var message = new SubmitApplicationRequest
            {
                ApplicationId = Guid.NewGuid()
            };

            _bus.PublishMessage(message);
        }
    }
}