namespace SFA.Apprenticeships.Infrastructure.Monitor
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Address.IoC;
    using Azure.Common.IoC;
    using Common.IoC;
    using Consumers;
    using Elastic.Common.IoC;
    using IoC;
    using LocationLookup.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using Postcode.IoC;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using UserDirectory.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private MonitorControlQueueConsumer _monitorControlQueueConsumer;

        public override void Run()
        {
            Logger.Debug("Monitor Process Run Called");

            if (!Initialise())
            {
                return;
            }

            while (true)
            {
                try
                {
                    _monitorControlQueueConsumer.CheckScheduleQueue().Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.WarnException("CommunicationException from MonitorSchedulerConsumer", ce);
                }
                catch (TimeoutException te)
                {
                    Logger.WarnException("TimeoutException from MonitorSchedulerConsumer", te);
                }
                catch (Exception ex)
                {
                    Logger.ErrorException("Exception from MonitorSchedulerConsumer", ex);
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }

        #region Helpers
        private bool Initialise()
        {
            try
            {
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
                    x.AddRegistry<AzureCommonRegistry>();
                    x.AddRegistry<ElasticsearchCommonRegistry>();
                    x.AddRegistry<UserRepositoryRegistry>();
                    x.AddRegistry<CandidateRepositoryRegistry>();
                    x.AddRegistry<ApplicationRepositoryRegistry>();
                    x.AddRegistry<VacancySearchRegistry>();
                    x.AddRegistry<LocationLookupRegistry>();
                    x.AddRegistry<AddressRegistry>();
                    x.AddRegistry<PostcodeRegistry>();
                    x.AddRegistry<UserDirectoryRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<MonitorRegistry>();
                });

                Logger.Debug("Monitor Process IoC initialized");

                _monitorControlQueueConsumer = ObjectFactory.GetInstance<MonitorControlQueueConsumer>();

                Logger.Debug("Monitor Process setup complete");

                return true;
            }
            catch (Exception ex)
            {
                Logger.FatalException("Monitor Process failed to initialise", ex);
                return false;
            }
        }

        public override bool OnStart()
        {
            Logger.Debug("Monitor Process OnStart called");

            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("Monitor Process OnStop called");

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }

        #endregion
    }
}
