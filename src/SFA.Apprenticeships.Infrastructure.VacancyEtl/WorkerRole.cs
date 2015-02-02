namespace SFA.Apprenticeships.Infrastructure.VacancyEtl
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
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Communication.IoC;
    using StructureMap;
    using VacancyIndexer.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Vacancy Processor";
        private VacancyEtlControlQueueConsumer _vacancyEtlControlQueueConsumer;

        public override void Run()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _logger = ObjectFactory.GetInstance<ILogService>();
#pragma warning restore 0618

            _logger.Debug(ProcessName + " Run called");

            Initialise();

            while (true)
            {
                try
                {
                    _vacancyEtlControlQueueConsumer.CheckScheduleQueue().Wait();
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

        private void Initialise()
        {
            _logger.Debug(ProcessName + " initialising...");

            VersionLogging.SetVersion();

            try
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
                    x.AddRegistry<VacancyIndexerRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<AzureCacheRegistry>();
                    x.AddRegistry<MemoryCacheRegistry>();
                    x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                    x.AddRegistry<GatewayVacancyEtlRegistry>();
                    x.AddRegistry<ElasticsearchCommonRegistry>();
                    x.AddRegistry<ApplicationRepositoryRegistry>();
                    x.AddRegistry<CommunicationRepositoryRegistry>();
                });
#pragma warning restore 0618

                _logger.Debug(ProcessName + " IoC initialised");

#pragma warning disable 618
                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 618
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(VacancySummaryPageConsumerAsync)), "VacancyEtl");
                _logger.Debug("Rabbit subscriptions setup");

#pragma warning disable 618
                _vacancyEtlControlQueueConsumer = ObjectFactory.GetInstance<VacancyEtlControlQueueConsumer>();
#pragma warning restore 618

                _logger.Debug(ProcessName + " initialisation complete");
            }
            catch (Exception ex)
            {
                _logger.Error(ProcessName + " failed to initialise", ex);
                throw;
            }
        }

        public override bool OnStart()
        {
            _logger.Debug(ProcessName + " OnStart called");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            _logger.Debug(ProcessName + " OnStop called");

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