namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using Candidate;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using VacancySearch;

    [Serializable]
    public class ApplicationViewModel
    {
        public VacancyDetailViewModel VacancyDetail { get; set; } //TODO Make this the summary info
        public CandidateViewModel Candidate { get; set; }
        public ApplicationStatuses Status { get; set; }
        public Guid ApplicationViewId { get; set; }
    }
}