namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using SFA.Apprenticeships.Application.Common.Helpers;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class SortByLocationSpecification<TModel> : AbstractSpecification<TModel, ISortableGeoLocation>, ISortableSpecification<TModel>
    {
        public SortByLocationSpecification(Expression<Func<TModel, ISortableGeoLocation>> fieldname)
            :base(fieldname)
        {
        }

        public int SortOrder { get; set; }

        public override string Build(TModel entity)
        {
            var term = Term.Invoke(entity);
            if (term != null && term.SortEnabled)
            {
                var sort = new StringBuilder("{\"_geo_distance\":{");
                sort.AppendFormat("\"{0}\":{{", Fieldname);
                sort.AppendFormat("\"lat\":{0},", term.lat);
                sort.AppendFormat("\"lon\":{0}", term.lon);
                sort.AppendFormat("}},\"order\":\"{0}\",\"unit\":\"mi\"}}}}", term.SortDirection.GetDescription());

                return sort.ToString();
            }

            return string.Empty;
        }
    }
}
