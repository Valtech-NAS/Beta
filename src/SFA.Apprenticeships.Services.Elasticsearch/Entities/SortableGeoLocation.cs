using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class SortableGeoLocation : ISortableGeoLocation
    {
        public double Distance { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }

        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}
