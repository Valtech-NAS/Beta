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
    using Mongo.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using Repositories.Applications.Entities;

    public class ApprenticeshipApplicationDiagnosticsRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipApplicationDiagnosticsRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public ApprenticeshipApplicationDiagnosticsRepository(IConfigurationManager configurationManager, IMapper mapper, ICandidateReadRepository candidateReadRepository, ILogService logger)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
            _mapper = mapper;
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public IEnumerable<ApprenticeshipApplicationDetail> GetApplicationsForValidCandidatesWithUnsetLegacyId()
        {
            var applicationsForValidCandidatesWithUnsetLegacyId = new List<ApprenticeshipApplicationDetail>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(60);

            var applicationWithUnsetLegacyId = Collection.AsQueryable().Where(a => a.Status == ApplicationStatuses.Submitting && a.DateUpdated < outsideLikelyUpdateTime && a.LegacyApplicationId == 0);

            foreach (var mongoApprenticeshipApplicationDetail in applicationWithUnsetLegacyId)
            {
                var candidate = _candidateReadRepository.Get(mongoApprenticeshipApplicationDetail.CandidateId);
                //Exclude any applications associated with a candidate that does not yet have a valid legacy candidate id.
                //These candidates need updating first before any applications will go through.
                if (candidate.LegacyCandidateId == 0) continue;

                var apprenticeshipApplicationDetail = _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoApprenticeshipApplicationDetail);
                _logger.Debug("Apprenticeship application {0} is associated with a valid candidate but does not have a valid legacy application id from the legacy service", apprenticeshipApplicationDetail.EntityId);
                applicationsForValidCandidatesWithUnsetLegacyId.Add(apprenticeshipApplicationDetail);
            }

            return applicationsForValidCandidatesWithUnsetLegacyId;
        }

        public IEnumerable<CandidateApprenticeshipApplicationDetail> GetSubmittedApplicationsWithUnsetLegacyId()
        {
            var submittedApplicationsWithUnsetLegacyId = new List<CandidateApprenticeshipApplicationDetail>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(60);

            var applicationWithUnsetLegacyId = Collection.AsQueryable().Where(a => a.Status == ApplicationStatuses.Submitted && a.DateUpdated < outsideLikelyUpdateTime && a.LegacyApplicationId == 0);

            foreach (var mongoApprenticeshipApplicationDetail in applicationWithUnsetLegacyId)
            {
                var candidate = _candidateReadRepository.Get(mongoApprenticeshipApplicationDetail.CandidateId);
                //Exclude any applications associated with a candidate that does not yet have a valid legacy candidate id.
                //These candidates need updating first before any applications will go through.
                if (candidate.LegacyCandidateId == 0) continue;

                var apprenticeshipApplicationDetail = _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoApprenticeshipApplicationDetail);
                var candidateApprenticeshipApplicationDetail = new CandidateApprenticeshipApplicationDetail
                {
                    Candidate = candidate,
                    ApprenticeshipApplicationDetail = apprenticeshipApplicationDetail
                };
                _logger.Debug("Apprenticeship application {0} is associated with a valid candidate but does not have a valid legacy application id from the legacy service", apprenticeshipApplicationDetail.EntityId);
                submittedApplicationsWithUnsetLegacyId.Add(candidateApprenticeshipApplicationDetail);
            }

            return submittedApplicationsWithUnsetLegacyId;
        }

        public IEnumerable<string> GetDraftApplicationVacancyIds()
        {
            return Collection.Distinct("Vacancy._id", Query.EQ("Status", ApplicationStatuses.Draft)).Select(v => v.ToString());
        }

        public void UpdateApplicationStatus(ApprenticeshipApplicationDetail applicationDetail, ApplicationStatuses newApplicationStatus)
        {
            Collection.Update(Query.EQ("EntityId", applicationDetail.EntityId), new UpdateDocument {{"$set", new BsonDocument("Status", newApplicationStatus)}});
        }

        public void UpdateLegacyApplicationId(ApprenticeshipApplicationDetail applicationDetail, int legacyApplicationId)
        {
            Collection.Update(Query.EQ("EntityId", applicationDetail.EntityId), new UpdateDocument { { "$set", new BsonDocument("LegacyApplicationId", legacyApplicationId) } });
        }
    }
}
