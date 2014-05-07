using System;
using System.Collections.Generic;
using SFA.Apprenticeships.Repository.Elasticsearch.Attributes;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    [ElasticSearchMapping(Name = "vacancy")]
    public class Vacancy
    {
        //[ElasticSearchIndex(Name="fullname", Index=ElasticSearchIndexType.NotAnalyzed)]
        public string Employer { get; set; }

        [ElasticSearchType("double")]
        public decimal Wage { get; set; }

        [ElasticSearchType("double")]
        public decimal Hours { get; set; }

        public string VacancyType { get; set; }
        //[ElasticSearchIndex(Name="fullname", Index=ElasticSearchIndexType.NotAnalyzed)]
        public string Provider { get; set; }
        //[ElasticSearchIndex(Name = "full", Index = ElasticSearchIndexType.NotAnalyzed)]
        public string Title { get; set; }
        //[ElasticSearchIndex(Name = "full", Index = ElasticSearchIndexType.NotAnalyzed)]
        public string Postcode { get; set; }
        public string District { get; set; }

        public List<Attrib> Criteria { get; set; }

        [ElasticSearchType("geo_point")]
        public GeoPoint Location { get; set; }

        [ElasticSearchType(Name = "date", Format = "yyyy-MM-dd")]
        public DateTime StartDate { get; set; }
        [ElasticSearchType(Name = "date", Format = "yyyy-MM-dd")]
        public DateTime EndDate { get; set; }
        [ElasticSearchType(Name = "date", Format = "yyyy-MM-dd")]
        public DateTime PostDate { get; set; }
    }
}
