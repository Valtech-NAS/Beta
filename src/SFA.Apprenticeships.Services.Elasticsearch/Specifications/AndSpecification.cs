using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class AndSpecification<T> : IConstraintSpecification<T>
    {
        private readonly IList<IConstraintSpecification<T>> _specs;
 
        public AndSpecification(IList<IConstraintSpecification<T>> specifications)
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
            var ands = new StringBuilder();

            foreach (
                var build in _specs
                        .Select(spec => spec.Build(entity))
                        .Where(build => !string.IsNullOrEmpty(build)))
            {
                if (ands.Length > 0)
                {
                    ands.Append(",");
                }

                ands.Append(build);
            }

            return ands.Length > 0 ? string.Format("\"and\":[{0}]", ands) : string.Empty;
        }
    }
}
