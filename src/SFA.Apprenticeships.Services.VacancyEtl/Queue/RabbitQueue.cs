namespace SFA.Apprenticeships.Services.VacancyEtl.Queue
{
    using System.Reflection;
    using EasyNetQ;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Common.Messaging.RabbitMQ;

    public static class RabbitQueue
    {
        public static IBus Setup()
        {
            var bus = Transport.CreateBus();
            var bs = new BootstrapSubcribers(bus);
            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), typeof(VacancySummaryConsumerAsync).Name);

            return bus;
        }
    }
}
