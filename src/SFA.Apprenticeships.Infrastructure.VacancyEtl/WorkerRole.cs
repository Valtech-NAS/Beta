namespace SFA.Apprenticeships.Infrastructure.VacancyEtl
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using EasyNetQ;
    using NLog;
    using Azure.Common.IoC;
    using Elastic.Common.IoC;
    using RabbitMq.Interfaces;
    using Consumers;
    using Common.IoC;
    using LegacyWebServices.IoC;
    using RabbitMq.IoC;
    using IoC;
    using VacancyIndexer.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private VacancySchedulerConsumer _vacancySchedulerConsumer;

        public override void Run()
        {
            Logger.Debug("Vacancy Etl Process Run Called");

            if (!Initialise())
            {
                Logger.Fatal("Vacancy Etl Process failed to initialise");
                return;
            }

            while (true)
            {
                try
                {
                    var task = _vacancySchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException from legacy web services", ce);
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException from legacy web services", te);
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception from VacancySchedulerConsumer", ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        private bool Initialise()
        {
            try
            {
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
                    x.AddRegistry<AzureCommonRegistry>();
                    x.AddRegistry<VacancyIndexerRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<LegacyWebServicesRegistry>();
                    x.AddRegistry<VacancyEtlRegistry>();
                    x.AddRegistry<ElasticsearchCommonRegistry>();
                });

                Logger.Debug("Vacancy Etl Process IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(VacancySummaryConsumerAsync)), "VacancyEtl");
                Logger.Debug("Rabbit subscriptions setup");

                _vacancySchedulerConsumer = ObjectFactory.GetInstance<VacancySchedulerConsumer>();

                Logger.Debug("Vacancy Etl Process setup complete");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Vacancy Etl Process failed to initialise", ex);
                return false;
            }
        }

        public override bool OnStart()
        {
            Logger.Debug("Vacancy Etl Process OnStart called");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("Vacancy Etl Process OnStop called");

            // Kill the bus which will kill any subscriptions
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }
    }
}
