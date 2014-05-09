using System;
using System.Linq.Expressions;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class TermSpecification<TModel> : AbstractSpecification<TModel, ISortable<string>>, IConstraintSpecification<TModel>
    {
        public TermSpecification(Expression<Func<TModel, ISortable<string>>> fieldname)
            :base(fieldname)
        {
        }

        public override string Build(TModel parameters)
        {
            var term = Term.Invoke(parameters);
            return term != null && !string.IsNullOrEmpty(term.Value)
                ? string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", Fieldname, term.Value.ToLower())
                : string.Empty;
        }
    }
}
