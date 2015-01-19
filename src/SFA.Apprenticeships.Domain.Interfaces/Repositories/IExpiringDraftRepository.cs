namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Communication;

    public interface IExpiringDraftRepository
    {
        void Upsert(ExpiringDraft expiringDraft);

        Dictionary<Guid, List<ExpiringDraft>> GetCandidatesDailyDigest();
    }
}
