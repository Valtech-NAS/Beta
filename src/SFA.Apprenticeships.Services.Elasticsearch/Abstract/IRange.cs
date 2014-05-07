namespace SFA.Apprenticeships.Services.Elasticsearch.Abstract
{
    public interface IRange
    {
        bool HasValue { get; }
        object RangeFrom { get; set; }
        object RangeTo { get; set; }
    }

    public interface IRange<out T> : IRange
    {
        T From { get; }
        T To { get; }
    }
}
