namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class GeoLocationSpecification<TModel> : AbstractSpecification<TModel, ISortableGeoLocation>, IConstraintSpecification<TModel>
    {
        private readonly string _units;

        public GeoLocationSpecification(Expression<Func<TModel, ISortableGeoLocation>> fieldname)
            : base(fieldname)
        {
            _units = "mi"; //units.GetDescription();
        }

        public override string Build(TModel parameters)
        {
            var point = Term.Invoke(parameters);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (point == null || point.Distance == 0d)
            {
                return string.Empty;
            }

            var query = new StringBuilder();

            query.Append("{\"geo_distance\":{");
            query.AppendFormat("\"distance\":\"{0}{1}\",", point.Distance, _units);
            query.AppendFormat("\"{0}\":{{\"lat\":{1},\"lon\":{2}}}", Fieldname, point.lat, point.lon);
            query.Append("}}");

            return query.ToString();
        }
    }
}