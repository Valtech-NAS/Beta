namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IntegrationConsole
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading;
    using System.Xml.Serialization;
    using Azure.Common;
    using Azure.Common.IoC;
    using Common.IoC;
    using Consumers;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Microsoft.WindowsAzure.Storage.Queue;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;

    internal class Program
    {
        private static void Main(string[] args)
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<AzureCommonConsoleRegistry>();
                x.AddRegistry<VacancyIndexerRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<GatewayVacancyEtlRegistry>();
            });

            //if (args != null && args.Length > 0)
            //{
                //Any args means 
                var azureClient = ObjectFactory.GetInstance<IAzureCloudClient>();
                var queueItems = GetAzureScheduledMessagesQueue(1);
                azureClient.AddMessage("vacancysearchdatacontrol", queueItems.Dequeue());
                Console.WriteLine("Vacancy indexer control message added to queue");
            //}

            Console.WriteLine("Enter any key to start processing the queues");

            //while (!Console.KeyAvailable)
            //{
            //    Thread.Sleep(1000);
            //}

            //Console.WriteLine("Enter any key to quit");
            //Console.WriteLine("---------------------------------------------------------------");

            //var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
            //subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof (VacancySummaryConsumerAsync)),
            //    "VacancyEtl");
            //var vacancySchedulerConsumer = ObjectFactory.GetInstance<VacancyEtlControlQueueConsumer>();

            //while (!Console.KeyAvailable)
            //{
            //    try
            //    {
            //        var task = vacancySchedulerConsumer.CheckScheduleQueue();
            //        task.Wait();
            //    }
            //    catch (CommunicationException ce)
            //    {
            //        Console.WriteLine("CommunicationException returned from legacy web services, error: {0}", ce.Message);
            //    }
            //    catch (TimeoutException te)
            //    {
            //        Console.WriteLine("TimeoutException returned from legacy web services, error: {0}", te.Message);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Unknown Exception returned from VacancySchedulerConsumer", ex);
            //    }
            //    finally
            //    {
            //        Thread.Sleep(5*60*1000);
            //    }
            //}
        }

        private static Queue<CloudQueueMessage> GetAzureScheduledMessagesQueue(int count)
        {
            var queue = new Queue<CloudQueueMessage>();
            var serializer = new XmlSerializer(typeof (StorageQueueMessage));

            for (var i = count; i > 0; i--)
            {
                var storageScheduleMessage = new StorageQueueMessage
                {
                    ClientRequestId = Guid.NewGuid(),
                    ExpectedExecutionTime = DateTime.UtcNow,
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