using NLog;

namespace SFA.Apprenticeships.Infrastructure.VacancyEtl
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.Threading;
    using EasyNetQ;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Application.Interfaces.Messaging;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using RabbitMq.Interfaces;
    using Consumers;
    using SFA.Apprenticeships.Infrastructure.Azure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.IoC;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private VacancySchedulerConsumer _vacancySchedulerConsumer;
        private readonly static Logger Logger = LogManager.GetLogger(Constants.NamedLoggers.VacanyImporterLogger);

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Logger.Trace("EtlWorkerRole entry point called");

            if (!Initialise())
            {
                // TODO: Check this exits worker.
                return;
            }

            while (true)
            {
                try
                {
                    var task = _vacancySchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
                }
                catch (CommunicationException ce)
                {
                    Logger.Warn("CommunicationException returned from legacy web services, error: {0}", ce.Message);
                }
                catch (TimeoutException te)
                {
                    Logger.Warn("TimeoutException returned from legacy web services, error: {0}", te.Message);
                }
                catch (Exception ex)
                {
                    Logger.Error("Unknown Exception returned from VacancySchedulerConsumer", ex);
                }
                finally
                {
                    Thread.Sleep(60 * 1000);
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
                });

                Logger.Trace("IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(VacancySummaryConsumerAsync)), "VacancyEtl");
                Logger.Trace("Rabbit subscritions set up");

                _vacancySchedulerConsumer = new VacancySchedulerConsumer(
                    ObjectFactory.GetInstance<IMessageService<StorageQueueMessage>>(),
                    ObjectFactory.GetInstance<IVacancySummaryProcessor>());

                Logger.Trace("VacancySchedulerConsumer setup complete");
            }
            catch (Exception ex)
            {
                Logger.Error("Error initialising VacancyEtl Worker/Server", ex);
                return false;
            }

            return true;
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            // Kill the bus which will kill any subscriptions
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(5000);
            base.OnStop();
        }
    }
}