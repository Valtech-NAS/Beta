namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications
{
    using System;
    using System.Linq.Expressions;
    using SFA.Apprenticeships.Infrastructure.Common.Helpers;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class SortByFieldnameSpecification<TModel> : AbstractSpecification<TModel, ISortable>, ISortableSpecification<TModel>
    {
        public SortByFieldnameSpecification(Expression<Func<TModel, ISortable>> fieldname)
            :base(fieldname)
        {
        }

        public int SortOrder { get; set; }

        public override string Build(TModel entity)
        {
            var term = Term.Invoke(entity);
            if (term != null && term.SortEnabled)
            {
                return string.Format("{{\"{0}\":{{\"order\":\"{1}\"}}}}", Fieldname, term.SortDirection.GetDescription());
            }

            return string.Empty;
        }
    }
}
