namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;

    public class CommunicationRequestConsumerAsync : IConsumeAsync<CommunicationRequest>
    {
        public Task Consume(CommunicationRequest message)
        {
            // note, for now only candidate messages are being sent so assume entity ID is candidate ID
            var candidateId = message.EntityId;

            // note, some messages are mandatory - determined by type
            var isMandatory = message.MessageType == MessageTypes.TraineeshipApplicationSubmitted ||
                              message.MessageType == MessageTypes.ApprenticeshipApplicationSubmitted;

            //todo: check comms prefs for candidate and invoking SMS/Email dispatcher accordingly

            throw new NotImplementedException();
        }
    }
}
