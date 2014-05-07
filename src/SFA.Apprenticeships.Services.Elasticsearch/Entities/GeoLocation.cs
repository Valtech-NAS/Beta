using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class GeoLocation : IGeoPoint
    {
        public bool HasValue { get; set; }
        public double Distance { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
