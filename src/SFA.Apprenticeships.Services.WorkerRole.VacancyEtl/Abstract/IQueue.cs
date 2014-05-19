using System;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Abstract
{
    public interface IQueue
    {
        IQueueMessage GetMessage();
    }

    public interface IQueueMessage
    {
        bool HasMessage { get; set; }
        string Json { get; set; }
        string Id { get; set; }
        Guid Reference { get; set; }
    }
}
