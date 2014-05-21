using System;
using EasyNetQ.AutoSubscribe;
using SFA.Apprenticeships.Common.Entities.Vacancy;

namespace SFA.Apprenticeships.Services.VacancyEtl.Consumers
{
    public class VacancySummaryConsumerSync : IConsume<VacancySummary>
    {
        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryConsumerSync")]
        public void Consume(VacancySummary message)
        {
            //Console.WriteLine("TestMessageConsumerSync recieved message with TestString:" + message.TestString);
        }
    }
}
