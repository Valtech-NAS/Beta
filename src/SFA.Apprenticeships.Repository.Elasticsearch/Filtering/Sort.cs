using System;
using System.Text;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;
using SFA.Apprenticeships.Repository.Elasticsearch.Entities;
using SFA.Apprenticeships.Repository.Elasticsearch.Helpers;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Filtering
{
    /// <summary>
    /// Implements one level sort term. Needs enhancing with collection of ISort for multiple level sorts.
    /// </summary>
    public class Sort<T> where T : class, ISortTerm
    {
        private readonly T _sort;

        public Sort(T sort)
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }

            _sort = sort;
        }

        public string Create()
        {
            // Use a pattern for this!!
            switch (_sort.SortBy)
            {
                case SortByType.Fieldname:
                    return ByFieldname(_sort);

                case SortByType.Point:
                    return ByLocation((ISortLocation)_sort);

                default:
                    return string.Empty;
            }
        }

        private string ByLocation(ISortLocation point)
        {
            var sort = new StringBuilder("\"sort\":[{\"_geo_distance\":{");
            sort.AppendFormat("\"{0}\":{{", point.SortFieldname);
            sort.AppendFormat("\"lat\":{0},", point.Location.lat);
            sort.AppendFormat("\"lon\":{0}", point.Location.lon);
            sort.AppendFormat("}},\"order\":\"{0}\",\"unit\":\"mi\"}}}}]", point.SortDirection.GetDescription());

            return sort.ToString();
        }

        private string ByFieldname(ISortTerm term)
        {
            return string.Format("\"sort\":[{{\"{0}\":{{\"order\":\"{1}\"}}}}]", term.SortFieldname, term.SortDirection.GetDescription());
        }
    }
}
