namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using System;
    using Candidate;
    using VacancySearch;

    [Serializable]
    public class ApplicationViewModel
    {
        public VacancyDetailViewModel VacancyDetail { get; set; }

        public CandidateViewModel Candidate { get; set; }
    }
}