namespace SFA.Apprenticeships.CustomHosts.AsyncProcessorService
{
    using System.Net;
    using System.Threading;
    using EasyNetQ;
    using Infrastructure.Common.IoC;
    using Infrastructure.Communication.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.RabbitMq.Interfaces;
    using Infrastructure.RabbitMq.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using Infrastructure.Repositories.Users.IoC;
    using NLog;
    using Infrastructure.AsyncProcessor.Consumers;
    using Infrastructure.AsyncProcessor.IoC;
    using StructureMap;

    public partial class AsyncProcessorService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AsyncProcessorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.Debug("OnStart called.");

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            Initialise();
        }

        protected override void OnStop()
        {
            Logger.Debug("AsyncProcessor OnStop called.");

            // Kill the bus which will kill any subscriptions
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
#pragma warning restore 0618

            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));
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
                Logger.Error("AsyncProcessor Initialisation error.", e);
            }
        }

        private static void InitializeRabbitMQSubscribers()
        {
            const string asyncProcessorSubscriptionId = "AsyncProcessor";

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var bootstrapper = ObjectFactory.GetInstance<IBootstrapSubcribers>();
#pragma warning restore 0618

            Logger.Debug("RabbitMQ initialising");

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof (EmailRequestConsumerAsync)),
                asyncProcessorSubscriptionId);

            Logger.Debug("RabbitMQ initialised");
        }

        private static void InitializeIoC()
        {
            Logger.Debug("IoC container initialising");

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<AsyncProcessorRegistry>();
            });
#pragma warning restore 0618

            Logger.Debug("IoC container initialised");
        }
    }
}