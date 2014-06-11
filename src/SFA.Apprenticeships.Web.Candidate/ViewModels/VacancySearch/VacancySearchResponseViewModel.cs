namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;

    public class VacancySearchResponseViewModel
    {
        public int TotalHits { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }

        public IEnumerable<VacancySummaryResponse> Vacancies { get; set; }

        public VacancySearchViewModel VacancySearch { get; set; }

        public int Pages(int pageSize)
        {
            var pages = 1;
            if (pageSize > 0)
            {
                pages = TotalHits/pageSize;
                if (TotalHits%pageSize > 0) pages++;
            }

            return pages;
        }
    }
}