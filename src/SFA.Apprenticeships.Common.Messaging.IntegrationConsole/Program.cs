
namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;
    using System.Threading;
    using SFA.Apprenticeships.Common.Configuration;
    using SFA.Apprenticeships.Common.Configuration.Azure;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
    using SFA.Apprenticeships.Common.Messaging.Azure;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Services.VacancyEtl.Load;
    using SFA.Apprenticeships.Services.VacancyEtl.Queue;
    using StructureMap;

    class Program
    {
        static void Main(string[] args)
        {
            // This is a sample worker implementation. Replace with your logic.
            IoC.IoC.Initialize();
            ElasticsearchLoad<VacancySummary>.Setup(ObjectFactory.GetInstance<IElasticsearchService>());
            var bus = RabbitQueue.Setup();
            var client = new AzureCloudClient(new AzureConsoleConfig(ObjectFactory.GetInstance<IConfigurationManager>()));

            var vacancySchedulerConsumer = new VacancySchedulerConsumer(
                                                bus,
                                                client,
                                                ObjectFactory.GetInstance<IVacancySummaryService>());


            Console.WriteLine("Enter any key to quite");
            Console.WriteLine("---------------------------------------------------------------");

            while (!Console.KeyAvailable)
            {
                var task = vacancySchedulerConsumer.CheckScheduleQueue();
                task.Wait();
                Thread.Sleep(5000);
            }
        }
    }
}
