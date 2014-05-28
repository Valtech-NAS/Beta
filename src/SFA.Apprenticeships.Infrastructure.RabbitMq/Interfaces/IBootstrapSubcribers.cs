namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces
{
    using System.Reflection;

    public interface IBootstrapSubcribers
    {
        void LoadSubscribers(Assembly assembly, string subscriptionId);
    }
}