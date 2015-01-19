namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver.Builders;
    using NLog;

    public class ExpiringDraftRepository : CommunicationRepository<ExpiringDraft>, IExpiringDraftRepository
    {
        private readonly IMapper _mapper;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ExpiringDraftRepository(IConfigurationManager configurationManager, string collectionName, IMapper mapper)
            : base(configurationManager, collectionName)
        {
            _mapper = mapper;
        }

        public void Upsert(ExpiringDraft expiringDraft)
        {
            var mongoExpiringDraft = _mapper.Map<ExpiringDraft, MongoExpiringDraft>(expiringDraft);
            UpdateEntityTimestamps(mongoExpiringDraft);
            // Defaults to upsert
            Collection.Save(mongoExpiringDraft);
        }

        public Dictionary<Guid, List<ExpiringDraft>> GetCandidatesDailyDigest()
        {
            var mongoExpiringDrafts = Collection.FindAs<MongoExpiringDraft>(Query.EQ("IsSent", false));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoExpiringDraft>, IEnumerable<ExpiringDraft>>(mongoExpiringDrafts);
            return expiringDrafts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());
        }
    }
}
