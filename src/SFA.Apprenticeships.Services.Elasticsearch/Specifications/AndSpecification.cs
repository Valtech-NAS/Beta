using System;
using System.Collections.Generic;
using System.Text;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Specifications
{
    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly IList<ISpecification<T>> _specs;
 
        public AndSpecification(IList<ISpecification<T>> specifications)
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

            foreach (var spec in _specs)
            {
                var build = spec.Build(entity);
                if (!string.IsNullOrEmpty(build))
                {
                    if (ands.Length > 0)
                    {
                        ands.Append(",");
                    }

                    ands.Append(build);
                }
            }

            return ands.Length > 0 ? string.Format("\"and\":[{0}]", ands) : string.Empty;
        }
    }
}
