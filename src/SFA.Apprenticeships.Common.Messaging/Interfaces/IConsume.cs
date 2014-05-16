namespace SFA.Apprenticeships.Common.Messaging.Interfaces
{
    using System.Threading.Tasks;

    public interface IConsume<in T> where T : class
    {
        string SubscriptionId { get; }
        void Consume(T message);
    }

    public interface IConsumeAsync<in T> where T : class
    {
        string SubscriptionId { get; }
        Task Consume(T message);
    }

    public interface IConsumeTopicAsync<in T> where T : class
    {
        string SubscriptionId { get; }
        string Topic { get; }
        Task Consume(T message, string topic);
    }

    public interface IConsumeTopic<in T> where T : class
    {
        string SubscriptionId { get; }
        string Topic { get; }
        void Consume(T message, string topic);
    }
}
