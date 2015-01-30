namespace SFA.Apprenticeships.Application.Communication.Strategies
{
    using System;
    using System.Collections.Generic;
    using Interfaces.Communications;

    public interface ISendTraineeshipApplicationSubmittedStrategy
    {
        void Send(Guid candidateId, IEnumerable<CommunicationToken> tokens);
    }
}
