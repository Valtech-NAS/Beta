using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class QuerySpecification<T> : ISpecification<T>
    {
        private readonly IList<ISpecification<T>> _specs;

        public QuerySpecification(IList<ISpecification<T>> specifications)
        {
            // Use Guard
            if (specifications == null)
            {
                throw new ArgumentNullException("specifications");
            }

            _specs = specifications;
        }

        public string Build(T entity)
        {
            var sort = new StringBuilder();

            foreach (
                var build in _specs
                    .Where(x => x is ISortableSpecification<T>)
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
                    .Where(x => x is IConstraintSpecification<T>)
                    .Select(spec => spec.Build(entity))
                    .Where(build => !string.IsNullOrEmpty(build)))
            {
                if (constraint.Length > 0)
                {
                    constraint.Append(",");
                }

                constraint.Append(build);
            }

            var filter =string.Format("{{\"sort\":[{0}],\"query\":{{\"constant_score\":{{\"filter\":{{{1}}}}}}}}}", 
                sort,
                constraint);

            return filter;
        }
    }
}
