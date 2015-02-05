namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    public class ExpiringDraftRepository : CommunicationRepository<ExpiringDraft>, IExpiringDraftRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public ExpiringDraftRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "expiringdrafts")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public void Save(ExpiringDraft expiringDraft)
        {
            _logger.Debug("Calling repository to save expiring draft with Id={0}, CandidateId={1}, ApplicationId={2}", expiringDraft.EntityId, expiringDraft.CandidateId, expiringDraft.ApplicationId);
            
            var mongoExpiringDraft = _mapper.Map<ExpiringDraft, MongoExpiringDraft>(expiringDraft);
            
            UpdateEntityTimestamps(mongoExpiringDraft);
            mongoExpiringDraft.SentDateTime = mongoExpiringDraft.BatchId.HasValue ? mongoExpiringDraft.DateUpdated : null;

            _logger.Debug("Saved expiring draft to repository with Id={0}, CandidateId={1}, ApplicationId={2}", expiringDraft.EntityId, expiringDraft.CandidateId, expiringDraft.ApplicationId);

            Collection.Save(mongoExpiringDraft);
        }
         
        public void Delete(ExpiringDraft expiringDraft)
        {
            _logger.Debug("Calling repository to expiring draft with Id={0}", expiringDraft.EntityId);

            Collection.Remove(Query.EQ("_id", expiringDraft.EntityId));

            _logger.Debug("Deleted expiring draft with Id={0}", expiringDraft.EntityId);
        }

        public Dictionary<Guid, List<ExpiringDraft>> GetCandidatesDailyDigest()
        {
            _logger.Debug("Calling repository to get all candidates expiring drafts for their daily digest");

            var mongoExpiringDrafts = Collection.FindAs<MongoExpiringDraft>(Query.EQ("BatchId", BsonNull.Value));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoExpiringDraft>, IEnumerable<ExpiringDraft>>(mongoExpiringDrafts);

            var candidatesDailyDigest = expiringDrafts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());

            _logger.Debug("Found expiring drafts for {0} candidates", candidatesDailyDigest.Count);

            return candidatesDailyDigest;
        }
    }
}
