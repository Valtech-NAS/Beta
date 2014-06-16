namespace SFA.Apprenticeships.Infrastructure.VacancySearch
{
    using System.Globalization;
    using System.Linq;
    using Nest;
    using Application.Interfaces.Vacancy;
    using Application.Interfaces.Search;
    using Domain.Entities.Location;
    using Elastic.Common.Configuration;

    public class VacancySearchProvider : IVacancySearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        public VacancySearchProvider(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public SearchResults<VacancySummaryResponse> FindVacancies(string keywords, 
                                                                    Location location, 
                                                                    int pageNumber, 
                                                                    int pageSize, 
                                                                    int searchRadius,
                                                                    VacancySortType sortType)
        {
            int distanceSortItemIndex = 0;
            var client = _elasticsearchClientFactory.GetElasticClient();
            var indexName = _elasticsearchClientFactory.GetIndexNameForType(typeof (Elastic.Common.Entities.VacancySummary));
            var documentTypeName = _elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.VacancySummary));

            var search = client.Search<VacancySummaryResponse>(s =>
            {              
                s.Index(indexName);
                s.Type(documentTypeName);     
                s.Skip((pageNumber - 1) * pageSize);
                s.Take(pageSize);
                
                switch (sortType)
                {
                    case VacancySortType.Distance:
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                             .Unit(GeoUnit.mi).OnField(f => f.Location);
                            return g;
                        });
                        break;
                    case VacancySortType.ClosingDate:
                        distanceSortItemIndex = 1;
                        s.Sort(v => v.OnField(f => f.ClosingDate).Ascending());
                        //Need this to get the distance from the sort.
                        //Was trying to get distance in relevancy without this sort but can't .. yet
                        s.SortGeoDistance(g =>
                        {
                            g.PinTo(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                             .Unit(GeoUnit.mi).OnField(f => f.Location);
                            return g;   
                        });
                        break;
                    case VacancySortType.Relevancy:
                        //Using ScriptFields to calculate distance doesn't work
                        //See notes at foot of file
                        //s.Fields(f => f.Title, f => f.Description);
                        //s.ScriptFields(sf => 
                        //    sf.Add("distance2", sfd => sfd.Params(fp =>
                        //    {
                        //    fp.Add("lat", location.GeoPoint.Latitute);
                        //    fp.Add("lon", location.GeoPoint.Longitude);
                        //    return fp;
                        //}).Script("doc[\u0027location\u0027].distanceInMiles(lat,lon)")));
                        break;
                }

                if (location != null)
                {
                    s.Filter(f => f.GeoDistance(vs => vs.Location, descriptor => descriptor
                                            .Location(location.GeoPoint.Latitute, location.GeoPoint.Longitude)
                                            .Distance(searchRadius, GeoUnit.mi)));
                }

                if (!string.IsNullOrEmpty(keywords))
                {
                    s.Query(q =>
                    {
                        BaseQuery query = q.FuzzyLikeThis(flt => flt
                                            .OnFields(new[] { "title", "description", "employerName" })
                                            .LikeText(keywords)
                                            .PrefixLength(1)
                                            .MinimumSimilarity(2));
                        return query;
                    });    
                }
                
                return s;
            });

            var responses = search.Documents.ToList();
            if (sortType != VacancySortType.Relevancy)
            {
                responses.ForEach(r => r.Distance =
                                        double.Parse(search.Hits.Hits.First(h => h.Id == r.Id.ToString(CultureInfo.InvariantCulture))
                                        .Sorts.Skip(distanceSortItemIndex).First()
                                        .ToString()));
            }

            var results = new SearchResults<VacancySummaryResponse>(search.Total, pageNumber, responses);
            
            return results;
        }

        /*
        Can't get NEST to build the query that should work with the additional calculated distance field
        as the below request returns (it would work if we could define _source fields using NEST but can't
        see a way around it just now.
        GET vacancies_search/_search
        {
            "_source":{
                "include": [ "*"],
                "exclude": [ "other" ]
            },
            "query": {
              "bool": {
                "should": [
                  {
                    "fuzzy": {
                      "title": {
                        "boost": 2.0,
                        "min_similarity": 5.0,
                        "prefix_length": 1,
                        "value": "social"
                      }
                    }
                  },
                  {
                    "fuzzy": {
                      "description": {
                        "boost": 1.0,
                        "min_similarity": 2.0,
                        "prefix_length": 1,
                        "value": "social"
                      }
                    }
                  }
                ]
              }
            },
            "script_fields": {
            "distance2": {
              "script": "doc['location'].distanceInMiles(lat,lon)",
              "params": {
                "lat": 52.4009991288043,
                "lon": -1.50812239495425
              }
            }
          },
          "filter": {
            "geo_distance": {
              "distance": 20.0,
              "unit": "mi",
              "location": "52.4009991288043, -1.50812239495425"
            }
          }
        }
*/
    }
}
