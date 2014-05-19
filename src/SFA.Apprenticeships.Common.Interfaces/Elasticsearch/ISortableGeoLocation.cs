
namespace SFA.Apprenticeships.Common.Interfaces.Elasticsearch
{
    public interface ISortableGeoLocation : ISortable, IGeoPoint
    {
        double Distance { get; set; }
    }

}
