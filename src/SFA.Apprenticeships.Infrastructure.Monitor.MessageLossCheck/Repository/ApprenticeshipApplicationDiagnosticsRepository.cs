namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using MongoDB.Driver.Linq;
    using NLog;
    using Repositories.Applications.Entities;

    public class ApprenticeshipApplicationDiagnosticsRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipApplicationDiagnosticsRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public ApprenticeshipApplicationDiagnosticsRepository(IConfigurationManager configurationManager, IMapper mapper, ICandidateReadRepository candidateReadRepository)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
            _mapper = mapper;
            _candidateReadRepository = candidateReadRepository;
        }

        public IEnumerable<ApprenticeshipApplicationDetail> GetApplicationsForValidCandidatesWithUnsetLegacyId()
        {
            var applicationsForValidCandidatesWithUnsetLegacyId = new List<ApprenticeshipApplicationDetail>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(6);

            var applicationWithUnsetLegacyId = Collection.AsQueryable().Where(a => a.Status == ApplicationStatuses.Submitting && a.DateUpdated < outsideLikelyUpdateTime && a.LegacyApplicationId == 0);

            foreach (var mongoApprenticeshipApplicationDetail in applicationWithUnsetLegacyId)
            {
                var candidate = _candidateReadRepository.Get(mongoApprenticeshipApplicationDetail.CandidateId);
                //Exclude any applications associated with a candidate that does not yet have a valid legacy candidate id.
                //These candidates need updating first before any applications will go through.
                if (candidate.LegacyCandidateId == 0) continue;

                var apprenticeshipApplicationDetail = _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoApprenticeshipApplicationDetail);
                Logger.Debug("Apprenticeship application {0} is associated with a valid candidate but does not have a valid legacy application id from the legacy service", apprenticeshipApplicationDetail.EntityId);
                applicationsForValidCandidatesWithUnsetLegacyId.Add(apprenticeshipApplicationDetail);
            }

            return applicationsForValidCandidatesWithUnsetLegacyId;
        }
    }
}