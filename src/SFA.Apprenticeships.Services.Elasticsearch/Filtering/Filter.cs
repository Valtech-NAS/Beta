using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Filtering
{
    public class Filter<T> where T : class, ISortTerm
    {
        private readonly ISpecification<T> _specs;
        private readonly T _entity;

        public Filter(ISpecification<T> specs, T entity)
        {
            _specs = specs;
            _entity = entity;
        }

        public string Create() 
        {
            var sorting = new Sort<T>(_entity);
            var sort = sorting.Create();

            var querying = new Query<T>(_specs, _entity);
            var query = querying.Create();

            var filter = string.Format(
                "{{{0}{1}{2}}}",
                sort,
                sort.Length > 0 && query.Length > 0
                    ? ","
                    : string.Empty,
                query);

            return filter;
        }

        // Example
        //public string Query(QueryParameters query)
        //{
        //    var specs = new List<ISpecification<QueryParameters>>
        //    {
        //        new TermSpecification<QueryParameters>(q => q.Employer),
        //        new TermSpecification<QueryParameters>(q => q.Provider),
        //        new TermSpecification<QueryParameters>(q => q.Title),
        //        new TermSpecification<QueryParameters>(q => q.VacancyType),
        //        new RangeSpecification<QueryParameters>(q => q.Hours),
        //        new RangeSpecification<QueryParameters>(q => q.Wage),
        //        new GeoLocationSpecification<QueryParameters>(q => q.Location),
        //        new RangeSpecification<QueryParameters>(q => q.PostDate)
        //    };

        //    var filter = new AndSpecification<QueryParameters>(specs).Build(query);
        //}
    }
}
