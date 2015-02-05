namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Communication;

    public interface IExpiringApprenticeshipApplicationDraftRepository
    {
        void Save(ExpiringApprenticeshipApplicationDraft expiringDraft);

        void Delete(ExpiringApprenticeshipApplicationDraft expiringDraft);

        List<ExpiringApprenticeshipApplicationDraft> GetExpiringApplications(int vacancyId);

        Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>> GetCandidatesDailyDigest();
    }
}
