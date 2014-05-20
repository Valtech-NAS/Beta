using System;
using System.Reflection;
using EasyNetQ;
using SFA.Apprenticeships.Common.Messaging;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Abstract;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Queue
{
    class RabbitQueue : IQueue
    {
        private readonly IBus _bus;

        public RabbitQueue()
        {
            _bus = Transport.CreateBus();

            var bs = new Bootstrapper(_bus);
            bs.LoadConsumers(Assembly.GetExecutingAssembly(), "test_app");
        }

        public IQueueMessage GetMessage()
        {
            throw new NotImplementedException();
        }
    }
}
