using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Elasticsearch
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
