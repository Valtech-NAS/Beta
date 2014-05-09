using System;
using System.Linq.Expressions;
using System.Text;
using SFA.Apprenticeships.Services.Common.Helpers;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class SortByLocationSpecification<T> : ISortableSpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, ISortableGeoLocation> _sortTerm;

        public SortByLocationSpecification(Expression<Func<T, ISortableGeoLocation>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentNullException("fieldname");
                }

                _fieldname = memberExpression.Member.Name;              
                _sortTerm = fieldname.Compile();
            }
        }

        public string Build(T entity)
        {
            var term = _sortTerm.Invoke(entity);
            if (term != null && term.SortEnabled)
            {
                var sort = new StringBuilder("{\"_geo_distance\":{");
                sort.AppendFormat("\"{0}\":{{", _fieldname);
                sort.AppendFormat("\"lat\":{0},", term.lat);
                sort.AppendFormat("\"lon\":{0}", term.lon);
                sort.AppendFormat("}},\"order\":\"{0}\",\"unit\":\"mi\"}}}}", term.SortDirection.GetDescription());

                return sort.ToString();
            }

            return string.Empty;
        }
    }
}
