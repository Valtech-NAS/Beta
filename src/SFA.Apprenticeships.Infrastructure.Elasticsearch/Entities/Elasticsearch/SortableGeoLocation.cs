namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch
{
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class SortableGeoLocation : ISortableGeoLocation
    {
        public double Distance { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }

        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}
