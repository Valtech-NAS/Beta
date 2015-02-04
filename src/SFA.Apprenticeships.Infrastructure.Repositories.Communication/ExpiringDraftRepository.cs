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

    //todo: add logging (see other repo classes)
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
            var mongoExpiringDraft = _mapper.Map<ExpiringDraft, MongoExpiringDraft>(expiringDraft);
            
            UpdateEntityTimestamps(mongoExpiringDraft);
            mongoExpiringDraft.SentDateTime = mongoExpiringDraft.BatchId.HasValue ? mongoExpiringDraft.DateUpdated : null;

            Collection.Save(mongoExpiringDraft);
        }

        public void Delete(ExpiringDraft expiringDraft)
        {
            Collection.Remove(Query.EQ("_id", expiringDraft.EntityId));
        }

        public Dictionary<Guid, List<ExpiringDraft>> GetCandidatesDailyDigest()
        {
            var mongoExpiringDrafts = Collection.FindAs<MongoExpiringDraft>(Query.EQ("BatchId", BsonNull.Value));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoExpiringDraft>, IEnumerable<ExpiringDraft>>(mongoExpiringDrafts);

            return expiringDrafts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());
        }
    }
}
