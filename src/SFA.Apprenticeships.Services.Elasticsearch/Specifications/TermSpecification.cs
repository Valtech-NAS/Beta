using System;
using System.Linq.Expressions;
using SFA.Apprenticeships.Services.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    public class TermSpecification<T> : IConstraintSpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, ISortable<string>> _searchTerm;

        public TermSpecification(Expression<Func<T, ISortable<string>>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentNullException("fieldname");
                }

                _fieldname = memberExpression.Member.Name;              
                _searchTerm = fieldname.Compile();
            }
        }

        public string Build(T parameters)
        {
            var term = _searchTerm.Invoke(parameters);
            return term != null && !string.IsNullOrEmpty(term.Value)
                ? string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", _fieldname, term.Value.ToLower())
                : string.Empty;
        }
    }
}
