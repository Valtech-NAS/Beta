namespace SFA.Apprenticeships.Application.Communications
{
    using System;

    public interface ICommunicationProcessor
    {
        void SendDailyDigests(Guid batchId);

        //todo: 1.7: send daily "saved search" results
        //void SendSavedSearchResults(Guid batchId);
    }
}
