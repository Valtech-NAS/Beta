namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Communication;

    public interface IExpiringDraftRepository
    {
        void Save(ExpiringDraft expiringDraft);

        void Delete(ExpiringDraft expiringDraft);

        Dictionary<Guid, List<ExpiringDraft>> GetCandidatesDailyDigest();
    }
}
