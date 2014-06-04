namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Tests.Models
{
    using System;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class QueryTestModel
    {
        public ISortable<string> Postcode { get; set; }
        public ISortable<string> VacancyType { get; set; }
        public ISortable<string> Title { get; set; }
        public ISortableRange<DateTime> PostDate { get; set; }
        public ISortableRange<DateTime> VacancyDate { get; set; }
        public ISortableRange<double> Wage { get; set; }
        public ISortableRange<double> Hours { get; set; }
        public ISortable<string> Provider { get; set; }
        public ISortable<string> Employer { get; set; }
        public ISortableGeoLocation Location { get; set; }
    }
}