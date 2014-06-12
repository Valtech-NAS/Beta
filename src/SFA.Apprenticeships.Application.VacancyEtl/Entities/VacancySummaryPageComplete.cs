namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    
    /// <summary>
    /// Message to indicate last summary page has been processed.
    /// </summary>
    public class VacancySummaryUpdateComplete
    {
        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
