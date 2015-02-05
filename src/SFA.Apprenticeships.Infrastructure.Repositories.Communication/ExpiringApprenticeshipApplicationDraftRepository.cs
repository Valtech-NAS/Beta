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

    //todo: add logging
    public class ExpiringApprenticeshipApplicationDraftRepository : CommunicationRepository<ExpiringApprenticeshipApplicationDraft>, IExpiringApprenticeshipApplicationDraftRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public ExpiringApprenticeshipApplicationDraftRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "expiringdraftapplications")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public void Save(ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            var mongoExpiringDraft = _mapper.Map<ExpiringApprenticeshipApplicationDraft, MongoApprenticeshipApplicationExpiringDraft>(expiringDraft);
            UpdateEntityTimestamps(mongoExpiringDraft);
            mongoExpiringDraft.SentDateTime = mongoExpiringDraft.BatchId.HasValue ? mongoExpiringDraft.DateUpdated : null;
            Collection.Save(mongoExpiringDraft);
        }

        public void Delete(ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            Collection.Remove(Query.EQ("_id", expiringDraft.EntityId));
        }

        public List<ExpiringApprenticeshipApplicationDraft> GetExpiringApplications(int vacancyId)
        {
            var mongoExpiringDrafts = Collection.FindAs<MongoApprenticeshipApplicationExpiringDraft>(Query.EQ("VacancyId", vacancyId));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoApprenticeshipApplicationExpiringDraft>, IEnumerable<ExpiringApprenticeshipApplicationDraft>>(mongoExpiringDrafts);
            return expiringDrafts.ToList();
        }

        public Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>> GetCandidatesDailyDigest()
        {
            var mongoExpiringDrafts = Collection.FindAs<MongoApprenticeshipApplicationExpiringDraft>(Query.EQ("BatchId", BsonNull.Value));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoApprenticeshipApplicationExpiringDraft>, IEnumerable<ExpiringApprenticeshipApplicationDraft>>(mongoExpiringDrafts);
            return expiringDrafts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());
        }
    }
}
