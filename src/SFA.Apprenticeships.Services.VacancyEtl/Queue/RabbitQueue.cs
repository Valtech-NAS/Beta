namespace SFA.Apprenticeships.Services.VacancyEtl.Queue
{
    using System.Reflection;
    using EasyNetQ;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
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
