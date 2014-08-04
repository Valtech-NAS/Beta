namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Consumers;
    using EasyNetQ;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ApplicationSchedulerConsumer _applicationSchedulerConsumer;

        public override void Run()
        {
            Logger.Debug("Application Etl Process Run Called");

            if (!Initialise())
            {
                Logger.Fatal("Application Etl Process failed to initialise");
                return;
            }

            while (true)
            {
                try
                {
                    /* TODO: implement worker role
                    var task = _applicationSchedulerConsumer.CheckScheduleQueue();
                    task.Wait();
                    */
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
                /* TODO: implement worker role
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<CommonRegistry>();
                    x.AddRegistry<AzureCommonRegistry>();
                    x.AddRegistry<RabbitMqRegistry>();
                    x.AddRegistry<LegacyWebServicesRegistry>();
                    x.AddRegistry<ApplicationEtlRegistry>();
                });

                Logger.Debug("Application Etl Process IoC initialized");

                var subscriberBootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
                subscriberBootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(ApplicationSummaryConsumerAsync)), "ApplicationEtl");
                Logger.Debug("Rabbit subscriptions setup");
                */

                _applicationSchedulerConsumer = ObjectFactory.GetInstance<ApplicationSchedulerConsumer>();

                Logger.Debug("Application Etl Process setup complete");

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Application Etl Process failed to initialise", ex);
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
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }
    }
}
