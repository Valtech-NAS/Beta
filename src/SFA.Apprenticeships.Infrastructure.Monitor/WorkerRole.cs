namespace SFA.Apprenticeships.Infrastructure.Monitor
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Consumers;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using StructureMap;

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
                    etc.
                });

                Logger.Debug("Monitor Process IoC initialized");
                */

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

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("Monitor Process OnStop called");

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }
    }
}
