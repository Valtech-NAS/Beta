namespace SFA.Apprenticeships.Infrastructure.Communications
{
    using System;
    using System.Net;
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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using RabbitMq.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Communications Process";
        private CommunicationsControlQueueConsumer _communicationsControlQueueConsumer;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    var isEnabled = bool.Parse(CloudConfigurationManager.GetSetting("IsEnabled") ?? "true");

                    if (isEnabled)
                    {
                        _logger.Debug(ProcessName + " worker role enabled");
                        _communicationsControlQueueConsumer.CheckScheduleQueue().Wait();
                    }
                    else
                    {
                        _logger.Debug(ProcessName + " worker role disabled");
                    }

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
                x.AddRegistry<CommunicationsRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
            });

            _logger = ObjectFactory.GetInstance<ILogService>();
#pragma warning restore 0618
        }

        private void InitialiseRabbitMQSubscribers()
        {
#pragma warning disable 618
            //var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 618

            //subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(CommunicationsControlQueueConsumer)), "Communications");

#pragma warning disable 618
            _communicationsControlQueueConsumer = ObjectFactory.GetInstance<CommunicationsControlQueueConsumer>();
#pragma warning restore 618
        }
    }
}
