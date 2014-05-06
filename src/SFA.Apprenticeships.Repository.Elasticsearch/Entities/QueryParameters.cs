using System;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    public class QueryParameters
    {
        public QueryParameters()
        {
            Location = new GeoLocation();
            PostDate = new Range<DateTime>();
            VacancyDate = new Range<DateTime>();
            Wage = new Range<double>();
            Hours = new Range<double>();
        }

        public string Postcode { get; set; }
        public string VacancyType { get; set; }
        public string Title { get; set; }
        public IRange<DateTime> PostDate { get; set; }
        public IRange<DateTime> VacancyDate { get; set; }
        public IRange<double> Wage { get; set; }
        public IRange<double> Hours { get; set; }
        public string Provider { get; set; }
        public string Employer { get; set; }
        public GeoLocation Location { get; set; }

        public string SortBy { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}