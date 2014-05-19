
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Elasticsearch
{
    public class GeoPoint : IGeoPoint
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
