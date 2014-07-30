namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using Candidate;
    using Domain.Entities.Applications;
    using VacancySearch;

    [Serializable]
    public class ApplicationViewModel
    {
        public VacancyDetailViewModel VacancyDetail { get; set; }
        public CandidateViewModel Candidate { get; set; }
        public ApplicationStatuses Status { get; set; }
        public Guid ApplicationViewId { get; set; }
    }
}