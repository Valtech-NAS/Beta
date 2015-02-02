namespace SFA.Apprenticeships.Infrastructure.Monitor
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Address.IoC;
    using Application.Interfaces.Logging;
    using Azure.Common.IoC;
    using Common.IoC;
    using Consumers;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using LocationLookup.IoC;
    using Logging;
    using Logging.IoC;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Postcode.IoC;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Authentication.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using UserDirectory.IoC;
    using VacancySearch.IoC;

    public class WorkerRole : RoleEntryPoint
    {
        private static ILogService _logger;
        private const string ProcessName = "Monitor Process";
        private MonitorControlQueueConsumer _monitorControlQueueConsumer;

        public override void Run()
        {
            Initialise();

            while (true)
            {
                try
                {
                    _monitorControlQueueConsumer.CheckScheduleQueue().Wait();
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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<AzureCommonRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<AddressRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<MonitorRegistry>();
            });

            _logger = ObjectFactory.GetInstance<ILogService>();
#pragma warning restore 0618
        }

        private void InitialiseRabbitMQSubscribers()
        {
#pragma warning disable 618
            _monitorControlQueueConsumer = ObjectFactory.GetInstance<MonitorControlQueueConsumer>();
#pragma warning restore 618
        }
    }
}
