namespace SFA.Apprenticeships.Common.Messaging.Interfaces
{
    using System.Reflection;

    public interface IBootstrapSubcribers
    {
        void LoadSubscribers(Assembly assembly, string subscriptionId);
    }
}