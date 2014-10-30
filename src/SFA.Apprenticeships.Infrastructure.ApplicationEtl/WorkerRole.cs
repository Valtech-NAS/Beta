namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl
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
    using IoC;
    using LegacyWebServices.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ApplicationEtlControlQueueConsumer _applicationEtlControlQueueConsumer;

        public override void Run()
        {
            Logger.Debug("Application Etl Process Run Called");

            if (!Initialise())
            {
                return;
            }

            while (true)
            {
                try
                {
                    _applicationEtlControlQueueConsumer.CheckScheduleQueue().Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException from ApplicationSchedulerConsumer", ce);
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException from ApplicationSchedulerConsumer", te);
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception from ApplicationSchedulerConsumer", ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        private bool Initialise()
        {
            try
            {
#pragma warning disable 0618
                // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
                    x.AddRegistry<AzureCommonRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<LegacyWebServicesRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<ApplicationEtlRegistry>();
                    x.AddRegistry<ApplicationRepositoryRegistry>();
                });
#pragma warning restore 0618

                Logger.Debug("Application Etl Process IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(ApplicationStatusSummaryConsumerAsync)), "ApplicationEtl");
                Logger.Debug("Rabbit subscriptions setup complete");

                _applicationEtlControlQueueConsumer = ObjectFactory.GetInstance<ApplicationEtlControlQueueConsumer>();

                Logger.Debug("Application Etl Process setup complete");

                return true;
            }
            catch (Exception ex)
            {
                Logger.Fatal("Application Etl Process failed to initialise", ex);
                return false;
            }
        }

        public override bool OnStart()
        {
            Logger.Debug("Application Etl Process OnStart called");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("Application Etl Process OnStop called");

            // Kill the bus which will kill any subscriptions
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
#pragma warning restore 0618

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }
    }
}
