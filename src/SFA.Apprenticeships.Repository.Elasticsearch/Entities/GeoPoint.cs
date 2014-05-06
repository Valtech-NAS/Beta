using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Entities
{
    public class GeoPoint : IGeoPoint
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
