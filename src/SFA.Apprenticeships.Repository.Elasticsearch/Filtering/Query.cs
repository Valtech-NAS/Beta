using System;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Filtering
{
    public class Query<T> where T : class 
    {
        private readonly ISpecification<T> _specs;
        private readonly T _entity;

        public Query(ISpecification<T> specs, T entity)
        {
            if (specs == null)
            {
                throw new ArgumentNullException("specs");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _specs = specs;
            _entity = entity;
        }

        public string Create()
        {
            var filter = _specs.Build(_entity);

            return filter.Length > 0
                ? string.Format("\"query\":{{\"constant_score\":{{\"filter\":{{{0}}}}}}}", filter)
                : string.Empty;
        }
    }
}
