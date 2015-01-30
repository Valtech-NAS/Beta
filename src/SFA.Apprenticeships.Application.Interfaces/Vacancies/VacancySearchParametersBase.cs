namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Locations;
    using Search;

    public abstract class VacancySearchParametersBase : SearchParametersBase
    {
        public Location Location { get; set; }

        public int SearchRadius { get; set; }

        public VacancySearchSortType SortType { get; set; }

        public string VacancyReference { get; set; }

        public override string ToString()
        {
            return string.Format("Location:{0}, PageNumber:{1}, PageSize:{2}, SearchRadius:{3}, SortType:{4}, VacancyReference:{5}", 
                Location, PageNumber, PageSize, SearchRadius, SortType, VacancyReference);
        }        
    }
}
