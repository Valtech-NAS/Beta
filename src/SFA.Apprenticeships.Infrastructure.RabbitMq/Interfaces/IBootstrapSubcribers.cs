namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces
{
    using System.Reflection;
    using StructureMap;

    public interface IBootstrapSubcribers
    {
        void LoadSubscribers(Assembly assembly, string subscriptionId, IContainer container);
    }
}