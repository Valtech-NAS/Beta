namespace SFA.Apprenticeships.Application.VacancyEtl
{
    public interface IProcessControlQueue<T>
    {
        T GetMessage();

        void DeleteMessage(string id, string popReceipt);

        void AddMessage(T queueMessage); //todo: only used in test harness
    }
}
