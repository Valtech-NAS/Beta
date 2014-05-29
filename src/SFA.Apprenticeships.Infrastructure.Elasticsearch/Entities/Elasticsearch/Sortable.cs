namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities.Elasticsearch
{
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class Sortable<T> : ISortable<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }

            set
            {
                _value = value;
                HasValue = true;
            }
        }

        public bool HasValue { get; private set; }
        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}
