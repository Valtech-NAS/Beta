using SFA.Apprenticeships.Repository.Elasticsearch.Entities;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Abstract
{
    public interface ISortTerm
    {
        SortByType SortBy { get; set; }
        SortDirectionType SortDirection { get; set; }
        string SortFieldname { get; set; }
    }

    public interface ISortLocation : ISortTerm
    {
        GeoLocation Location { get; set; }
    }
}
