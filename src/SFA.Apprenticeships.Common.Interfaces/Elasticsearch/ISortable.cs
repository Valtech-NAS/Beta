using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch;

namespace SFA.Apprenticeships.Common.Interfaces.Elasticsearch
{
    public interface ISortable
    {
        bool SortEnabled { get; set; }
        SortDirectionType SortDirection { get; set; }
    }

    public interface ISortable<T> : ISortable
    {
        T Value { get; set; }
    }
}
