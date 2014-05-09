using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class SortingSpecification<T> : ISortableSpecification<T>
    {
        private readonly IList<ISortableSpecification<T>> _specs;

        public SortingSpecification(IList<ISortableSpecification<T>> specifications)
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
            var sorting = new StringBuilder();

            foreach (
                var build in
                    _specs.Select(spec => spec.Build(entity))
                        .Where(build => !string.IsNullOrEmpty(build)))
            {
                if (sorting.Length > 0)
                {
                    sorting.Append(",");
                }

                sorting.Append(build);
            }

            return sorting.Length > 0 ? sorting.ToString() : string.Empty;
        }
    }
}
