namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Communications;
    using Commands;
    using EasyNetQ.AutoSubscribe;

    public class CommunicationRequestConsumerAsync : IConsumeAsync<CommunicationRequest>
    {
        private readonly List<CommunicationCommand> _communicationCommands = new List<CommunicationCommand>();

        public CommunicationRequestConsumerAsync(IEnumerable<CommunicationCommand> communicationCommands)
        {
            _communicationCommands = new List<CommunicationCommand>(communicationCommands);
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CommunicationRequestConsumerAsync")]
        public Task Consume(CommunicationRequest message)
        {
            return Task.Run(() => _communicationCommands.First(cc => cc.CanHandle(message)).Handle(message));
        }
    }
}
