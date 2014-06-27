namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Applications
{
    using Candidate;

    public class ApplicationViewModel
    {
        public string VacancyTitle { get; set; }

        public string VacancySummary { get; set; }

        public string EmployerName { get; set; }

        public CandidateViewModel Candidate { get; set; }
    }
}