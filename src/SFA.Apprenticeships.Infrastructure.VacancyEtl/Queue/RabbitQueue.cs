namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Queue
{
    using System.Reflection;
    using EasyNetQ;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers;
    using StructureMap;

    public static class RabbitQueue
    {
        public static IBus Setup()
        {
            var bus = ObjectFactory.GetInstance<IBus>();
            var bs = ObjectFactory.GetInstance<IBootstrapSubcribers>();
            bs.LoadSubscribers(Assembly.GetExecutingAssembly(), typeof(VacancySummaryConsumerAsync).Name);

            return bus;
        }
    }
}
