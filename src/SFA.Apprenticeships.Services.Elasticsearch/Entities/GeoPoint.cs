using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class GeoPoint : IGeoPoint
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
