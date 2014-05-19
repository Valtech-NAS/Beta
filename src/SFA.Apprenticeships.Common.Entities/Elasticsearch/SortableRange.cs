using System;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.Enums.Elasticsearch;

namespace SFA.Apprenticeships.Common.Entities.Elasticsearch
{
    public class SortableRange<T> : ISortableRange<T> where T : struct
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

        public bool SortEnabled { get; set; }
        public SortDirectionType SortDirection { get; set; }
    }
}