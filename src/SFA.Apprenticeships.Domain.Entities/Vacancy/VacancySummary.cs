using SFA.Apprenticeships.Domain.Entities.Location;

namespace SFA.Apprenticeships.Domain.Entities.Vacancy
{
    using System;
   
    //TODO : Unflatten / Create Domain!
    public class VacancySummary
    {
        public int Id { get; set; }

        public string Framework { get; set; }

        public string Title { get; set; }

        public VacancyType VacancyType { get; set; }

        public DateTime Created { get; set; }

        public DateTime ClosingDate { get; set; }

        public string EmployerName { get; set; }

        public string ProviderName { get; set; }

        public int NumberOfPositions { get; set; }

        public string Description { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }

        public string LocalAuthority { get; set; }

        public GeoPoint Location { get; set; }

        public VacancyLocationType VacancyLocationType { get; set; }

        public string VacancyUrl { get; set; }
    }
}