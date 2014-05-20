using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Load;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Queue;

namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            ulong i = 1;
            Common.IoC.IoC.Initialize();
            ElasticsearchLoad<VacancySummary>.Setup();
            var bus = RabbitQueue.Setup();

            Console.WriteLine("Enter 'q' to quite and any antthing else to send a test message");
            Console.WriteLine("---------------------------------------------------------------");

            var input = Console.ReadLine();

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
