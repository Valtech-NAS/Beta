namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Xml.Serialization;
    using Microsoft.WindowsAzure.Storage.Queue;
    using SFA.Apprenticeships.Common.Configuration;
    using SFA.Apprenticeships.Common.Configuration.Azure;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
    using SFA.Apprenticeships.Common.Messaging.Azure;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Services.VacancyEtl.Entities;
    using SFA.Apprenticeships.Services.VacancyEtl.Load;
    using SFA.Apprenticeships.Services.VacancyEtl.Queue;
    using StructureMap;

    class Program
    {
        static void Main(string[] args)
        {
            // This is a sample worker implementation. Replace with your logic.
            Common.IoC.IoC.Initialize();
            ElasticsearchLoad<VacancySummary>.Setup(ObjectFactory.GetInstance<IElasticsearchService>());
            var bus = RabbitQueue.Setup();
            var client = new AzureCloudClient(new AzureConsoleConfig(ObjectFactory.GetInstance<IConfigurationManager>()));

            var vacancySchedulerConsumer = new VacancySchedulerConsumer(
                                                bus,
                                                client,
                                                ObjectFactory.GetInstance<IVacancySummaryService>());


            Console.WriteLine("Enter any key to quite");
            Console.WriteLine("---------------------------------------------------------------");

            var queueItems = GetAzureScheduledMessagesQueue(1);
            client.AddMessage("vacancysearchdatacontrol", queueItems.Dequeue());

            while (!Console.KeyAvailable)
            {
                var task = vacancySchedulerConsumer.CheckScheduleQueue();
                task.Wait();
                Thread.Sleep(5000);
            }
        }

        private static Queue<CloudQueueMessage> GetAzureScheduledMessagesQueue(int count)
        {
            var queue = new Queue<CloudQueueMessage>();
            var serializer = new XmlSerializer(typeof(StorageQueueMessage));

            for (int i = count; i > 0; i--)
            {
                var storageScheduleMessage = new StorageQueueMessage
                {
                    ClientRequestId = Guid.NewGuid().ToString(),
                    SchedulerJobId = i.ToString()
                };

                string message;
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, storageScheduleMessage);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    message = sr.ReadToEnd();
                }

                var cloudMessage = new CloudQueueMessage(message);
                queue.Enqueue(cloudMessage);
            }

            queue.Enqueue(null);
            return queue;
        }
    }
}
