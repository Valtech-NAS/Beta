namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Vacancies;

    public abstract class SearchParametersBase
    {
        public Location Location { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int SearchRadius { get; set; }

        public VacancySortType SortType { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public string VacancyReference { get; set; }

        public override string ToString()
        {
            return string.Format("Location:{0}, PageNumber:{1}, PageSize:{2}, SearchRadius:{3}, SortType:{4}, LocationType:{5}, VacancyReference:{6}", Location, PageNumber, PageSize, SearchRadius, SortType, VacancyLocationType, VacancyReference);
        }
    }
}
