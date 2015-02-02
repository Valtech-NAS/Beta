namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading;
    using Application.Interfaces.Logging;
    using Azure.Common.IoC;
    using Caching.Azure.IoC;
    using Caching.Memory.IoC;
    using Common.Configuration;
    using Common.IoC;
    using Consumers;
    using EasyNetQ;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Application Processor";
        private ApplicationEtlControlQueueConsumer _applicationEtlControlQueueConsumer;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    _applicationEtlControlQueueConsumer.CheckScheduleQueue().Wait();
                }
                catch (CommunicationException ce)
                {
                    _logger.Warn("CommunicationException from " + ProcessName, ce);
                }
                catch (TimeoutException te)
                {
                    _logger.Warn("TimeoutException from  " + ProcessName, te);
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception from  " + ProcessName, ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            // Stop consumers
#pragma warning disable 618
            ObjectFactory.GetInstance<ApplicationStatusSummaryPageConsumerAsync>().Stop();
            ObjectFactory.GetInstance<ApplicationStatusSummaryConsumerAsync>().Stop();
#pragma warning restore 618

            // Kill the bus which will kill any subscriptions
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
#pragma warning restore 0618

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }

        private void Initialise()
        {
            VersionLogging.SetVersion();

            try
            {
                InitializeIoC();
                InitialiseRabbitMQSubscribers();
            }
            catch (Exception ex)
            {
                if (_logger != null) _logger.Error(ProcessName + " failed to initialise", ex);
                throw;
            }
        }

        private static void InitializeIoC()
        {
            var config = new ConfigurationManager();
            var useCacheSetting = config.TryGetAppSetting("UseCaching");
            bool useCache;
            bool.TryParse(useCacheSetting, out useCache);

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<AzureCacheRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<ApplicationEtlRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
            });

            _logger = ObjectFactory.GetInstance<ILogService>();
#pragma warning restore 0618
        }

        private void InitialiseRabbitMQSubscribers()
        {
#pragma warning disable 618
            var bootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 618

            _logger.Debug("RabbitMQ initialising");

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(ApplicationStatusSummaryConsumerAsync)), "ApplicationEtl");

            _logger.Debug("RabbitMQ initialised");

#pragma warning disable 618
            _applicationEtlControlQueueConsumer = ObjectFactory.GetInstance<ApplicationEtlControlQueueConsumer>();
#pragma warning restore 618
        }
    }
}
