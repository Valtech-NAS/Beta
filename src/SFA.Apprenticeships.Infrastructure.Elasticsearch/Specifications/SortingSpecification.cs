namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class SortingSpecification<TModel> : ISortableSpecification<TModel>
    {
        private readonly IList<ISortableSpecification<TModel>> _specs;

        public SortingSpecification(IList<ISortableSpecification<TModel>> specifications)
        {
            // Use Guard
            if (specifications == null)
            {
                throw new ArgumentNullException("specifications");
            }

            _specs = specifications;
        }

        public int SortOrder { get; set; }

        public string Build(TModel entity)
        {
            var sorting = new StringBuilder();

            foreach (
                var build in _specs
                        .OrderBy(s => s.SortOrder)
                        .Select(spec => spec.Build(entity))
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
