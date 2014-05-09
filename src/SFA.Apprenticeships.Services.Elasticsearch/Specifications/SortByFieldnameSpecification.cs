using System;
using System.Linq.Expressions;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class SortByFieldnameSpecification<T> : ISortableSpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, ISortable> _sortTerm;

        public SortByFieldnameSpecification(Expression<Func<T, ISortable>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentNullException("fieldname");
                }

                _fieldname = memberExpression.Member.Name;
                _sortTerm = fieldname.Compile();
            }
        }

        public int SortOrder { get; set; }

        public string Build(T entity)
        {
            var term = _sortTerm.Invoke(entity);
            if (term != null && term.SortEnabled)
            {
                return string.Format("{{\"{0}\":{{\"order\":\"{1}\"}}}}", _fieldname, term.SortDirection);
            }

            return string.Empty;
        }
    }
}
