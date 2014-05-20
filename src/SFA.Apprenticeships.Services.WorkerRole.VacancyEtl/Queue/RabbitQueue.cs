using System.Reflection;
using SFA.Apprenticeships.Common.Messaging;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Queue
{
    public static class RabbitQueue
    {
        public static void Setup()
        {
            var bus = Transport.CreateBus();
            var bs = new Bootstrapper(bus);
            bs.LoadConsumers(Assembly.GetExecutingAssembly(), typeof(VacancySummaryConsumerAsync).Name);
        }
    }
}
