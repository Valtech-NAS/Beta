namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using Search;

    public class ApprenticeshipSearchParameters : VacancySearchParametersBase
    {
        public string Keywords { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public string Sector { get; set; }

        public string[] Frameworks { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipSearchField SearchField { get; set; }
        public override string ToString()
        {
            var joinedFrameworks = (Frameworks == null || Frameworks.Length == 0)
                ? string.Empty
                : string.Join(",", Frameworks);
            return string.Format("{0}, Keywords:{1}, ApprenticeshipLevel:{2}, Sector:{3}, Frameworks:{4}, LocationType:{5}", base.ToString(), Keywords, ApprenticeshipLevel, Sector, joinedFrameworks, VacancyLocationType);
        }
    }
}
