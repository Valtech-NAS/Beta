using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class ElasticSortable<T> : ISortable<T>
    {
        private T _value;

        public bool HasValue { get; private set; }

        public T Value
        {
            get { return _value; }

            set
            {
                _value = value;
                HasValue = true;
            }
        }

        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}
