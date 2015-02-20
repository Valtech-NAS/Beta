namespace SFA.Apprenticeships.CustomHosts.AsyncProcessorService
{
    using System.Net;
    using System.Threading;
    using EasyNetQ;
    using Infrastructure.Common.IoC;
    using Infrastructure.Communication.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.RabbitMq.Interfaces;
    using Infrastructure.RabbitMq.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.AsyncProcessor.Consumers;
    using Infrastructure.AsyncProcessor.IoC;
    using Infrastructure.Caching.Memory.IoC;
    using StructureMap;

    public partial class AsyncProcessorService : ServiceBase
    {
        private StructureMap.IContainer _container;

        public AsyncProcessorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            Initialise();
        }

        protected override void OnStop()
        {
            // Kill the bus which will kill any subscriptions
            _container.GetInstance<IBus>().Advanced.Dispose();
            
            // Give it 5 seconds to finish processing any in flight subscriptions.
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private void Initialise()
        {
            InitializeIoC();
            InitializeRabbitMQSubscribers();
        }

        private void InitializeRabbitMQSubscribers()
        {
            const string asyncProcessorSubscriptionId = "AsyncProcessor";

            var bootstrapper = _container.GetInstance<IBootstrapSubcribers>();

            bootstrapper.LoadSubscribers(Assembly.GetAssembly(typeof(EmailRequestConsumerAsync)), asyncProcessorSubscriptionId, _container);
        }

        private void InitializeIoC()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CommunicationRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<AsyncProcessorRegistry>();
            });
        }
    }
}
