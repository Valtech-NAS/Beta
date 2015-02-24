namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.SendContactMessageStrategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendContactMessageStrategyTests
    {
        private readonly Mock<ICommunicationService> _communicationService = new Mock<ICommunicationService>();
        private readonly Mock<IConfigurationManager> _configurationManager = new Mock<IConfigurationManager>();

        [Test]
        public void SendContactMessage()
        {
            const string helpdeskAddress = "helpdesk@gmail.com";

            var strategy = new Application.Candidate.Strategies.SendContactMessageStrategy(
                _communicationService.Object, _configurationManager.Object);

            _configurationManager.Setup(cm => cm.GetAppSetting<string>("HelpdeskEmailAddress")).Returns(helpdeskAddress);

            strategy.SendMessage(new ContactMessage());

            _communicationService.Verify(cs => cs.SendContactMessage(
                It.IsAny<Nullable<Guid>>(), 
                MessageTypes.CandidateContactMessage, 
                It.Is<IEnumerable<CommunicationToken>>(ct => ct.Count() == 5 && ct.First().Value == helpdeskAddress)));
        }
    }
}