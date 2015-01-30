namespace SFA.Apprenticeships.Infrastructure.Communications
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
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
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using RabbitMq.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Communication.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private CommunicationsControlQueueConsumer _communicationsControlQueueConsumer;

        public override void Run()
        {
            Logger.Debug("Communications Process Run Called");

            Initialise();

            while (true)
            {
                try
                {
                    var isEnabled = bool.Parse(CloudConfigurationManager.GetSetting("IsEnabled") ?? "true");

                    if (isEnabled)
                    {
                        Logger.Debug("Communications worker role enabled");
                        _communicationsControlQueueConsumer.CheckScheduleQueue().Wait();
                    }
                    else
                    {
                        Logger.Debug("Communications worker role disabled");
                    }

                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException from CommunicationsSchedulerConsumer", ce);
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException from CommunicationsSchedulerConsumer", te);
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception from CommunicationsSchedulerConsumer", ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        private void Initialise()
        {
            try
            {
                VersionLogging.SetVersion();

                var config = new ConfigurationManager();
                var useCacheSetting = config.TryGetAppSetting("UseCaching");
                bool useCache;
                bool.TryParse(useCacheSetting, out useCache);

#pragma warning disable 0618
                // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
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
#pragma warning restore 0618

                Logger.Debug("Communications Process IoC initialized");

#pragma warning disable 618
                //var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 618
                //subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(CommunicationsControlQueueConsumer)), "Communications");
                //Logger.Debug("Rabbit subscriptions setup complete");

#pragma warning disable 618
                _communicationsControlQueueConsumer = ObjectFactory.GetInstance<CommunicationsControlQueueConsumer>();
#pragma warning restore 618

                Logger.Debug("Communications Process setup complete");
            }
            catch (Exception ex)
            {
                Logger.Fatal("Communications Process failed to initialise", ex);
                throw;
            }
        }

        public override bool OnStart()
        {
            Logger.Debug("Communications Process OnStart called");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("Communications Process OnStop called");

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
