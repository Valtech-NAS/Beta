using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class QuerySpecification<TModel> : ISpecification<TModel>
    {
        private readonly IList<ISpecification<TModel>> _specs;

        public QuerySpecification(IList<ISpecification<TModel>> specifications)
        {
            // Use Guard
            if (specifications == null)
            {
                throw new ArgumentNullException("specifications");
            }

            _specs = specifications;
        }

        public string Build(TModel entity)
        {
            var sort = new StringBuilder();
            foreach (
                var build in _specs
                    .Where(x => x is ISortableSpecification<TModel>)
                    .Select(spec => spec.Build(entity))
                    .Where(build => !string.IsNullOrEmpty(build)))
            {
                if (sort.Length > 0)
                {
                    sort.Append(",");
                }

                sort.Append(build);
            }

            var constraint = new StringBuilder();
            foreach (
                var build in _specs
                    .Where(x => x is IConstraintSpecification<TModel>)
                    .Select(spec => spec.Build(entity))
                    .Where(build => !string.IsNullOrEmpty(build)))
            {
                if (constraint.Length > 0)
                {
                    constraint.Append(",");
                }

                constraint.Append(build);
            }

            var filter = string.Format(
                "{{\"sort\":[{0}],\"query\":{{\"constant_score\":{{\"filter\":{{{1}}}}}}}}}",
                sort,
                constraint);

            return filter;
        }
    }
}
