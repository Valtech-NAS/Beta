
using System.ComponentModel;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;
using SFA.Apprenticeships.Common.Entities.Elasticsearch;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public class VacancyAddress
    {
        public VacancyAddress()
        {
            Location = new GeoPoint();
        }

        [Description("AddressLine1")]
        public string AddressLine1 { get; set; }

        [Description("AddressLine2")]
        public string AddressLine2 { get; set; }

        [Description("AddressLine3")]
        public string AddressLine3 { get; set; }

        [Description("AddressLine4")]
        public string AddressLine4 { get; set; }

        [Description("AddressLine5")]
        public string AddressLine5 { get; set; }

        [Description("Town")]
        public string Town { get; set; }

        [Description("County")]
        public string County { get; set; }

        [Description("PostCode")]
        public string PostCode { get; set; }

        [Description("LocalAuthority")]
        public string LocalAuthority { get; set; }

        [ElasticsearchType("geo_point")]
        public IGeoPoint Location { get; set; }
    }
}


