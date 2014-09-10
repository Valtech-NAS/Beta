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
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using UserDirectory.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private MonitorSchedulerConsumer _monitorSchedulerConsumer;

        public override void Run()
        {
            Logger.Debug("Monitor Process Run Called");

            if (!Initialise())
            {
                Logger.Fatal("Monitor Process failed to initialise");
                return;
            }

            while (true)
            {
                try
                {
                    var task = _monitorSchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
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
                    x.AddRegistry<MonitorRegistry>();
                });

                Logger.Debug("Monitor Process IoC initialized");

                _monitorSchedulerConsumer = ObjectFactory.GetInstance<MonitorSchedulerConsumer>();

                Logger.Debug("Monitor Process setup complete");

                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Monitor Process failed to initialise", ex);
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
