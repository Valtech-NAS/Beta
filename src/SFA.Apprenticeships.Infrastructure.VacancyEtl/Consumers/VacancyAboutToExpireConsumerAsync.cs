namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using NLog;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public class VacancyAboutToExpireConsumerAsync : IConsumeAsync<VacancyAboutToExpire>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public VacancyAboutToExpireConsumerAsync()
        {
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancyAboutToExpireConsumerAsync")]
        public Task Consume(VacancyAboutToExpire vacancy)
        {
            Logger.Debug("Received vacancy about to expire message.");

            return Task.Run(() =>
            {
                
            });
        }
    }
}