
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public class VacancyAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }

        public string LocalAuthority { get; set; }
        public IGeoPoint Location { get; set; }
    }
}


