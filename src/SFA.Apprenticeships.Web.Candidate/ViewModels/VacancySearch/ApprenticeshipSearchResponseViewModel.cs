namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipSearchResponseViewModel : ViewModelBase
    {
        public ApprenticeshipSearchResponseViewModel(string message) : base(message)
        {
        }

        public ApprenticeshipSearchResponseViewModel()
        {
        }

        public long TotalLocalHits { get; set; }
        public long TotalNationalHits { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ApprenticeshipVacancySummaryViewModel> Vacancies { get; set; }
        public ApprenticeshipSearchViewModel VacancySearch { get; set; }

        public int PrevPage
        {
            get
            {
                if (VacancySearch == null) return 1;

                return VacancySearch.PageNumber == 1 ? 0 : VacancySearch.PageNumber - 1;
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

                if (VacancySearch.LocationType == ApprenticeshipLocationType.NonNational)
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

        public SelectList ResultsPerPageSelectList
        {
            get { return VacancySearch.ResultsPerPageSelectList; }
        }

        public SelectList Distances
        {
            get { return VacancySearch.Distances; }
        }

        public SelectList SortTypes
        {
            get { return VacancySearch.SortTypes; }
        }

        public ApprenticeshipSearchViewModel[] LocationSearches
        {
            get { return VacancySearch.LocationSearches; }
        }

        public SelectList ApprenticeshipLevels
        {
            get { return VacancySearch.ApprenticeshipLevels; }
        }
    }
}