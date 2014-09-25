namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using Domain.Entities.Vacancies;

    public class VacancySearchResponseViewModel : ViewModelBase
    {
        public VacancySearchResponseViewModel(string message) : base(message)
        {
        }

        public VacancySearchResponseViewModel()
        {
        }

        public long TotalLocalHits { get; set; }
        public long TotalNationalHits { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<VacancySummaryViewModel> Vacancies { get; set; }
        public VacancySearchViewModel VacancySearch { get; set; }

        public int PrevPage
        {
            get
            {
                if (VacancySearch == null) return 1;

                return VacancySearch.PageNumber == 1 ? 1 : VacancySearch.PageNumber - 1;
            }
        }

        public int NextPage
        {
            get
            {
                if (VacancySearch == null) return 1;

                var pages = Pages;
                return VacancySearch.PageNumber == pages ? pages : VacancySearch.PageNumber + 1;
            }
        }

        public int Pages
        {
            get
            {
                var pages = 1;
                if (PageSize <= 0) {return pages;}

                if (VacancySearch.LocationType == VacancyLocationType.NonNational)
                {
                    pages = (int)TotalLocalHits / PageSize;
                    if (TotalLocalHits % PageSize > 0) pages++;
                }
                else
                {
                    pages = (int)TotalNationalHits / PageSize;
                    if (TotalNationalHits % PageSize > 0) pages++;
                }

                return pages;
            }
        }
    }
}