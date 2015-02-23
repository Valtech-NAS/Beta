namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;

    public class TraineeshipMetricsRepository : GenericMongoClient<MongoCandidate>, ITraineeshipMetricsRepository
    {
        public TraineeshipMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Applications.mongoDB", "traineeships")
        {
        }

        public long GetApplicationCount()
        {
            return Collection.Count();
        }

        public long GetApplicationsPerCandidateCount()
        {
            return Collection.Distinct("CandidateId").Count();
        }
    }
}
