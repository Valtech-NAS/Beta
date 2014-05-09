using System;
using System.Linq.Expressions;
using System.Text;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class GeoLocationSpecification<T> : IConstraintSpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, ISortableGeoLocation> _searchTerm;
        private readonly string _units;

        public GeoLocationSpecification(Expression<Func<T, ISortableGeoLocation>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                _fieldname = memberExpression.Member.Name;
                _searchTerm = fieldname.Compile();
                _units = "mi"; //units.GetDescription();
            }
        }

        public string Build(T parameters)
        {
            var point = _searchTerm.Invoke(parameters);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (point == null || point.Distance == 0d)
            {
                return string.Empty;
            }

            var query = new StringBuilder();

            query.Append("{\"geo_distance\":{");
            query.AppendFormat("\"distance\":\"{0}{1}\",", point.Distance, _units);
            query.AppendFormat("\"{0}\":{{\"lat\":{1},\"lon\":{2}}}", _fieldname, point.lat, point.lon);
            query.Append("}}");

            return query.ToString();
        }
    }
}