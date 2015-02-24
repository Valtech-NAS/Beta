namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Infrastructure.Repositories.Applications.Entities;
    using Mongo.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    public class TraineeshipApplicationDiagnosticsRepository : GenericMongoClient<MongoTraineeshipApplicationDetail>, ITraineeshipApplicationDiagnosticsRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public TraineeshipApplicationDiagnosticsRepository(IConfigurationManager configurationManager, IMapper mapper, ICandidateReadRepository candidateReadRepository, ILogService logger)
            : base(configurationManager, "Applications.mongoDB", "traineeships")
        {
            _mapper = mapper;
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public IEnumerable<TraineeshipApplicationDetail> GetApplicationsForValidCandidatesWithUnsetLegacyId()
        {
            var applicationsForValidCandidatesWithUnsetLegacyId = new List<TraineeshipApplicationDetail>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(60);

            var applicationWithUnsetLegacyId = Collection.AsQueryable().Where(a => (a.DateUpdated == null || a.DateUpdated < outsideLikelyUpdateTime) && a.LegacyApplicationId == 0);

            foreach (var mongoTraineeshipApplicationDetail in applicationWithUnsetLegacyId)
            {
                var candidate = _candidateReadRepository.Get(mongoTraineeshipApplicationDetail.CandidateId);
                //Exclude any applications associated with a candidate that does not yet have a valid legacy candidate id.
                //These candidates need updating first before any applications will go through.
                if (candidate.LegacyCandidateId == 0) continue;

                var traineeshipApplicationDetail = _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoTraineeshipApplicationDetail);
                _logger.Debug("Traineeship application {0} is associated with a valid candidate but does not have a valid legacy application id from the legacy service", traineeshipApplicationDetail.EntityId);
                applicationsForValidCandidatesWithUnsetLegacyId.Add(traineeshipApplicationDetail);
            }

            return applicationsForValidCandidatesWithUnsetLegacyId;
        }

        public IEnumerable<CandidateTraineeshipApplicationDetail> GetSubmittedApplicationsWithUnsetLegacyId()
        {
            var submittedApplicationsWithUnsetLegacyId = new List<CandidateTraineeshipApplicationDetail>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(60);

            var applicationWithUnsetLegacyId = Collection.AsQueryable().Where(a => (a.DateUpdated == null || a.DateUpdated < outsideLikelyUpdateTime) && a.LegacyApplicationId == 0);

            foreach (var mongoTraineeshipApplicationDetail in applicationWithUnsetLegacyId)
            {
                var candidate = _candidateReadRepository.Get(mongoTraineeshipApplicationDetail.CandidateId);
                //Exclude any applications associated with a candidate that does not yet have a valid legacy candidate id.
                //These candidates need updating first before any applications will go through.
                if (candidate.LegacyCandidateId == 0) continue;

                var traineeshipApplicationDetail = _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoTraineeshipApplicationDetail);
                var candidateTraineeshipApplicationDetail = new CandidateTraineeshipApplicationDetail
                {
                    Candidate = candidate,
                    TraineeshipApplicationDetail = traineeshipApplicationDetail
                };
                _logger.Debug("Traineeship application {0} is associated with a valid candidate but does not have a valid legacy application id from the legacy service", traineeshipApplicationDetail.EntityId);
                submittedApplicationsWithUnsetLegacyId.Add(candidateTraineeshipApplicationDetail);
            }

            return submittedApplicationsWithUnsetLegacyId;
        }

        public void UpdateLegacyApplicationId(TraineeshipApplicationDetail applicationDetail, int legacyApplicationId)
        {
            Collection.Update(Query.EQ("EntityId", applicationDetail.EntityId), new UpdateDocument { { "$set", new BsonDocument("LegacyApplicationId", legacyApplicationId) } });
        }
    }
}