namespace SFA.Apprenticeships.Infrastructure.VacancyEtl
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading;
    using Azure.Common.IoC;
    using Common.IoC;
    using Communication.IoC;
    using Consumers;
    using EasyNetQ;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private VacancyEtlControlQueueConsumer _vacancyEtlControlQueueConsumer;

        public override void Run()
        {
            Logger.Debug("Vacancy Etl Process Run Called");

            if (!Initialise())
            {
                return;
            }

            while (true)
            {
                try
                {
                    _vacancyEtlControlQueueConsumer.CheckScheduleQueue().Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.WarnException("CommunicationException from legacy web services", ce);
                }
                catch (TimeoutException te)
                {
                    Logger.WarnException("TimeoutException from legacy web services", te);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Exception from VacancySchedulerConsumer", ex);
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
                    x.AddRegistry<GatewayVacancyEtlRegistry>();
                    x.AddRegistry<ElasticsearchCommonRegistry>();
                });

                Logger.Debug("Vacancy Etl Process IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof (VacancySummaryConsumerAsync)),
                    "VacancyEtl");
                Logger.Debug("Rabbit subscriptions setup");

                _vacancyEtlControlQueueConsumer = ObjectFactory.GetInstance<VacancyEtlControlQueueConsumer>();

                Logger.Debug("Vacancy Etl Process setup complete");
                return true;
            }
            catch (Exception ex)
            {
                Logger.FatalException("Vacancy Etl Process failed to initialise", ex);
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