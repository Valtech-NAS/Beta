namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    public class VacancySearchViewModel
    {
        public string JobTitle { get; set; }

        public string Keywords { get; set; }

        public string Location { get; set; }

        public int WithinDistance { get; set; }
    }
}