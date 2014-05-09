
namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface ISortableGeoLocation : ISortable, IGeoPoint
    {
        double Distance { get; set; }
    }

}
