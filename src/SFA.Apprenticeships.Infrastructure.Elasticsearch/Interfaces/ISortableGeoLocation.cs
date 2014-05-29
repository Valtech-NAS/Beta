
namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces
{
    public interface ISortableGeoLocation : ISortable, IGeoPoint
    {
        double Distance { get; set; }
    }

}
