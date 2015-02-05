namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using System;

    public class ExpiringApprenticeshipApplicationDraft : BaseEntity
    {
        public Guid CandidateId { get; set; }
        
        public int VacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
    }
}
