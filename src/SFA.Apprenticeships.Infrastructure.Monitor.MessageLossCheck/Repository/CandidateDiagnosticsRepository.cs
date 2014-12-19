namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using MongoDB.Driver.Linq;
    using NLog;
    using Repositories.Candidates.Entities;

    public class CandidateDiagnosticsRepository : GenericMongoClient<MongoCandidate>, ICandidateDiagnosticsRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _userReadRepository;

        public CandidateDiagnosticsRepository(IConfigurationManager configurationManager, IMapper mapper, IUserReadRepository userReadRepository)
            : base(configurationManager, "Candidates.mongoDB", "candidates")
        {
            _mapper = mapper;
            _userReadRepository = userReadRepository;
        }

        public IEnumerable<Candidate> GetActivatedCandidatesWithUnsetLegacyId()
        {
            var activatedCandidatesWithUnsetLegacyId = new List<Candidate>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.Now.AddMinutes(6);
            
            var candidatesWithUnsetLegacyId = Collection.AsQueryable().Where(c => c.DateUpdated < outsideLikelyUpdateTime && c.LegacyCandidateId == 0);
            
            foreach (var mongoCandidate in candidatesWithUnsetLegacyId)
            {
                var user = _userReadRepository.Get(mongoCandidate.EntityId);
                if (user.ActivationCode != null) continue;
                
                var candidate = _mapper.Map<MongoCandidate, Candidate>(mongoCandidate);
                Logger.Debug("Candidate {0} is associated with an activated user but does not have a valid legacy candidate id from the legacy service", candidate.EntityId);
                activatedCandidatesWithUnsetLegacyId.Add(candidate);
            }

            return activatedCandidatesWithUnsetLegacyId;
        }
    }
}