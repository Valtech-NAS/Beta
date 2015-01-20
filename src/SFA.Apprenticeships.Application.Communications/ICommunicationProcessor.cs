namespace SFA.Apprenticeships.Application.Communications
{
    using System;

    public interface ICommunicationProcessor
    {
        void SendDailyDigests(Guid batchId);
    }
}
