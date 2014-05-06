namespace SFA.Apprenticeships.Repository.Elasticsearch.Abstract
{
    public interface IGeoPoint
    {
        // These property names must not be changed!
        double lon { get; set; }
        double lat { get; set; }
    }
}
