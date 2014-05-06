
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    public class GeoLocation : IGeoPoint
    {
        public bool HasValue { get; set; }
        public double Distance { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
