using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Services.Elasticsearch.Mapping;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Consumers;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Load;

namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            ulong i = 1;
            Common.IoC.IoC.Initialize();

            ElasticsearchLoad<VacancySummary>.Setup();

            var bus = Transport.CreateBus();
            var bs = new Bootstrapper(bus);
            bs.LoadConsumers(Assembly.GetAssembly(typeof(VacancySummaryConsumerSync)), typeof(VacancySummaryConsumerAsync).Name);

            Console.WriteLine("Enter 'q' to quite and any antthing else to send a test message");
            Console.WriteLine("---------------------------------------------------------------");

            string input = Console.ReadLine();

            while (input != "q")
            {
                var testMessage = new VacancySummary()
                {
                    UpdateReference = Guid.NewGuid(),
                    Id = i++,
                    Created = DateTime.Now,
                    ClosingDate = DateTime.Today.AddDays(30),
                };
                bus.Publish(testMessage);
                input = Console.ReadLine();
            }
        }
    }
}
