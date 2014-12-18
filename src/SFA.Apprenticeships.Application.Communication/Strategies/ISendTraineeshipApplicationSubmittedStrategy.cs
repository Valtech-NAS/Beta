namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Interfaces.Messaging;

    public interface ISendTraineeshipApplicationSubmittedStrategy
    {
        void Send(Candidate candidate, TraineeshipApplicationDetail traineeshipApplicationDetail, CandidateMessageTypes messageType,
            IEnumerable<KeyValuePair<CommunicationTokens, string>> tokens);
    }
}