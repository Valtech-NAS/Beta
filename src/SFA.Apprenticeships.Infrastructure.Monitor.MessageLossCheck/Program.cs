namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck
{
    using System;
    using Common.IoC;
    using EasyNetQ;
    using Elastic.Common.IoC;
    using IoC;
    using RabbitMq.IoC;
    using Repositories.Candidates.IoC;
    using Repositories.Users.IoC;
    using StructureMap;
    using Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
#pragma warning disable 618
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<MessageLogCheckRepository>();
            });

            var messageLossCheckTaskRunner = ObjectFactory.GetInstance<IMessageLossCheckTaskRunner>();
#pragma warning restore 618

            messageLossCheckTaskRunner.RunMonitorTasks();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

#pragma warning disable 618
            ObjectFactory.GetInstance<IBus>().Advanced.Dispose();
#pragma warning restore 618
        }
    }
}
