namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using System.Globalization;
    using Locations;

    public class VacancySummaryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public GeoPointViewModel Location { get; set; }

        public double Distance { get; set; }

        public string DistanceAsString
        {
            get
            {
                return Math.Round(Distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}