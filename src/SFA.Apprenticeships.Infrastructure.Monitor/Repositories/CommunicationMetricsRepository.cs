namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Communication.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class CommunicationMetricsRepository : GenericMongoClient<MongoApprenticeshipApplicationExpiringDraft>, ICommunicationMetricsRepository
    {
        public CommunicationMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Communications.mongoDB", "expiringdraftapplications")
        {
        }

        public int GetDraftApplicationEmailsSentToday()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);

            return Collection
                .AsQueryable()
                .Count(each => each.SentDateTime >= today && each.SentDateTime <= tomorrow);
        }
    }
}
