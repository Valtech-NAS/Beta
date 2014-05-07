using SFA.Apprenticeships.Services.Elasticsearch.Entities;

namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
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
