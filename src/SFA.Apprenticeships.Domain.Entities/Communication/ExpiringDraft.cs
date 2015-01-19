namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;

    public class ExpiringDraft : BaseEntity
    {
        public Guid ApplicationId { get; set; }

        public Guid CandidateId { get; set; }
        
        public int VacancyId { get; set; }

        public string VacancyName { get; set; }

        public DateTime ClosingDate { get; set; }

        public bool IsSent { get; set; }
    }
}
