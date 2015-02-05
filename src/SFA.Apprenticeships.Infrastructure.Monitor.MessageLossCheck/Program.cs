namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck
{
    using System;
    using Common.IoC;
    using EasyNetQ;
    using Elastic.Common.IoC;
    using IoC;
    using LegacyWebServices.IoC;
    using Logging.IoC;
    using RabbitMq.IoC;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using Tasks;
    using VacancySearch.IoC;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(false));
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<MessageLogCheckRepository>();
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });

            var messageLossCheckTaskRunner = container.GetInstance<IMessageLossCheckTaskRunner>();

            messageLossCheckTaskRunner.RunMonitorTasks();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            container.GetInstance<IBus>().Advanced.Dispose();
        }
    }
}
