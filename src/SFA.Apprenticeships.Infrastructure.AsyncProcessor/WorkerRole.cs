namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using Communication.IoC;
    using Consumers;
    using EasyNetQ;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using RabbitMq.Interfaces;
    using RabbitMq.IoC;
    using StructureMap;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();

        public override void Run()
        {
            Logger.Debug("AsyncProcessor Run called.");

            Initialise();

            _cancelSource.Token.WaitHandle.WaitOne();

            Logger.Debug("AsyncProcessor Run exiting.");
        }

        public override bool OnStart()
        {
            Logger.Debug("OnStart called.");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Logger.Debug("AsyncProcessor OnStop called.");

            // Kill the bus which will kill any subscriptions
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            _cancelSource.Cancel();

            base.OnStop();
        }

        private static void Initialise()
        {
            try
            {
                Logger.Debug("AsyncProcessor Initialising...");

                InitializeIoC();
                InitializeRabbitMQSubscribers();

                Logger.Debug("AsyncProcessor Initialised.");
            }
            catch (Exception e)
            {
                Logger.ErrorException("AsyncProcessor Initialisation error.", e);
            }
        }

        private static void InitializeRabbitMQSubscribers()
        {
            const string asyncProcessorSubscriptionId = "AsyncProcessor";
            var bootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();

            Logger.Debug("RabbitMQ initialising");

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof (EmailConsumerAsync)), asyncProcessorSubscriptionId);

            Logger.Debug("RabbitMQ initialised");
        }

        private static void InitializeIoC()
        {
            Logger.Debug("IoC container initialising");

            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            Logger.Debug("IoC container initialised");
        }
    }
}