using SFA.Apprenticeships.Application.VacancyEtl;
using SFA.Apprenticeships.Application.VacancyEtl.Entities;
using SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC;
using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services;

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
    using RabbitMq.Interfaces;
    using Consumers;
    using Azure.Common.IoC;
    using Common.IoC;
    using LegacyWebServices.IoC;
    using RabbitMq.IoC;
    using IoC;
    using VacancyIndexer.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private VacancySchedulerConsumer _vacancySchedulerConsumer;
        private readonly static Logger Logger = LogManager.GetLogger(Constants.NamedLoggers.VacanyImporterLogger);

        public override void Run()
        {
            Logger.Debug("EtlWorkerRole entry point called");

            if (!Initialise())
            {
                Logger.Warn("EtlWorkerRole failed to initialise");
                return;
            }

            while (true)
            {
                try
                {
                    var task = _vacancySchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
                    Thread.Sleep(TimeSpan.FromMinutes(5));
                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException from legacy web services", ce);
                    return;
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException from legacy web services", te);
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception from VacancySchedulerConsumer", ex);
                    return;
                }
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

                Logger.Debug("IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(VacancySummaryConsumerAsync)), "VacancyEtl");
                Logger.Debug("Rabbit subscriptions setup");

                _vacancySchedulerConsumer = ObjectFactory.GetInstance<VacancySchedulerConsumer>();

                Logger.Debug("VacancySchedulerConsumer setup complete");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error initialising VacancyEtl Worker/Server", ex);
                return false;
            }
        }

        public override bool OnStart()
        {
            Logger.Debug("EtlWorkerRole OnStart called");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("EtlWorkerRole OnStop called");

            // Kill the bus which will kill any subscriptions
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }
    }
}
