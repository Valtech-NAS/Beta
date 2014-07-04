namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;

    public class ApplicationDetail : BaseEntity
    {
        //todo: ApplicationDetail, status, etc.
        public int LegacyApplicationId { get; set; } //todo: temporary "weak link" to legacy
    }
}
