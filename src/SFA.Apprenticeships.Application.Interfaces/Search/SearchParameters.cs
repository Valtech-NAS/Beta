namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Vacancies;

    public class SearchParameters
    {
        public string Keywords { get; set; }
        public Location Location { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int SearchRadius { get; set; }
        public VacancySortType SortType { get; set; }
        public ApprenticeshipLocationType VacancyLocationType { get; set; }
    }
}