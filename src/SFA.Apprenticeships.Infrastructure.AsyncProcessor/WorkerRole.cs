namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using Application.Interfaces.Messaging;
    using Communication.IoC;
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NLog;
    using RabbitMq.IoC;
    using StructureMap;
    using Consumers;
    using RabbitMq.Interfaces;

    public class WorkerRole : RoleEntryPoint
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void Run()
        {
            Logger.Debug("Run called.");

            Initialise();
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
            Logger.Debug("OnStop called.");

            // Kill the bus which will kill any subscriptions
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            base.OnStop();
        }

        private static void Initialise()
        {
            try
            {
                Logger.Debug("Initialising...");

                InitializeIoC();
                InitializeRabbitMQSubscribers();

                Logger.Debug("Initialised.");
            }
            catch (Exception e)
            {
                Logger.Error("Initialisation error.", e);
            }
        }

        private static void InitializeRabbitMQSubscribers()
        {
            const string asyncProcessorSubscriptionId = "AsyncProcessor";
            var bootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();

            bootstrapper.LoadSubscribers(
                Assembly.GetAssembly(typeof(EmailConsumerAsync)), asyncProcessorSubscriptionId);

            Logger.Debug("RabbitMQ initialized.");
        }

        private static void InitializeIoC()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            Logger.Debug("IoC initialized.");
        }
    }
}
