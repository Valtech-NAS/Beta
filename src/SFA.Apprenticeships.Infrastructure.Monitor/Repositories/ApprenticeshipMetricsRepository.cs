namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Applications.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class ApprenticeshipMetricsRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipMetricsRepository
    {
        public ApprenticeshipMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
        }

        public int GetApplicationCount()
        {
            return (int)Collection.Count();
        }

        public int GetApplicationStateCount(ApplicationStatuses applicationStatus)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.Status == applicationStatus);
        }

        public int GetApplicationCountPerCandidate()
        {
            return Collection.Distinct("CandidateId").Count();
        }

        public int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus)
        {
            return Collection
                .AsQueryable()
                .Where(each => each.Status == applicationStatus)
                .Select(each => each.CandidateId)
                .Distinct()
                .ToList()
                .Count;
        }
    }
}
