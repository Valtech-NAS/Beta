namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.Communication.IoC;
    using StructureMap;

    [TestFixture]
    public class TwilioSmsDispatcherTests
    {
        private ISmsDispatcher _dispatcher;
        private ISmsDispatcher _voidSmsDispatcher;

        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            _dispatcher = ObjectFactory.GetNamedInstance<ISmsDispatcher>("TwilioSmsDispatcher");
            _voidSmsDispatcher = ObjectFactory.GetNamedInstance<ISmsDispatcher>("VoidSmsDispatcher");
#pragma warning restore 0618
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructTwillioEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<TwilioSmsDispatcher>(_dispatcher);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidEmailDispatcher()
        {
            Assert.IsNotNull(_voidSmsDispatcher);
            Assert.IsInstanceOf<VoidSmsDispatcher>(_voidSmsDispatcher);
        }

        [Test]
        public void ShouldSendSms()
        {
            var request = new SmsRequest
            {
                MessageType = MessageTypes.ApprenticeshipApplicationSubmitted,
                ToNumber = "+447972527913",
                Tokens = null
            };

            _dispatcher.SendSms(request);
        }
    }
}