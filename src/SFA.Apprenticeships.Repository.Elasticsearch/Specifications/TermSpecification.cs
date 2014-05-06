using System;
using System.Linq.Expressions;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Specifications
{
    public class TermSpecification<T> : ISpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, string> _searchTerm;

        public TermSpecification(Expression<Func<T, string>> fieldname)
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
            return string.IsNullOrEmpty(term) ? string.Empty : string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", _fieldname, term.ToLower());
        }
    }
}
