using System;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    public class Range<T> : IRange<T>
    {
        public bool HasValue { get; private set; }
        public T From { get; private set; }
        public T To { get; private set; }

        public object RangeFrom
        {
            get { return From; }

            set
            {
                if (value is T)
                {
                    From = (T)value;
                    HasValue = true;
                }
                else
                {
                    throw new ArgumentException(string.Format("value must be type '{0}'", typeof(T).Name));
                }
            }
        }

        public object RangeTo
        {
            get { return To; }

            set
            {
                if (value is T)
                {
                    To = (T)value;
                    HasValue = true;
                }
                else
                {
                    throw new ArgumentException(string.Format("value must be type '{0}'", typeof(T).Name));
                }
            }
        }
    }
}