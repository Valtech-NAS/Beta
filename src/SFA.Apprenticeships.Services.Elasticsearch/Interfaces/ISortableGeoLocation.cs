
namespace SFA.Apprenticeships.Services.Elasticsearch.Interfaces
{
    public interface ISortableGeoLocation : ISortable, IGeoPoint
    {
        double Distance { get; set; }
    }

}
