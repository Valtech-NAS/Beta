using System;
using System.Linq.Expressions;
using System.Text;
using SFA.Apprenticeships.Repository.Elasticsearch.Abstract;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Specifications
{
    public class RangeSpecification<T> : ISpecification<T>
    {
        private readonly string _fieldname;
        private readonly Func<T, IRange> _searchTerm;

        public RangeSpecification(Expression<Func<T, IRange>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                _fieldname = memberExpression.Member.Name;
                _searchTerm = fieldname.Compile();
            }
        }

        public string Build(T parameters)
        {
            var term = _searchTerm.Invoke(parameters);
            if (!term.HasValue)
            {
                return string.Empty;
            }

            var query = new StringBuilder("{\"range\":{");

            var genericType = term.GetType().UnderlyingSystemType.GenericTypeArguments[0];
            if (genericType == typeof (DateTime))
            {
                if ((DateTime) term.RangeTo == default(DateTime))
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1:yyyy-MM-dd}\"}}", _fieldname, term.RangeFrom);
                }
                else
                {
                    query.AppendFormat(
                        "\"{0}\":{{\"from\":\"{1:yyyy-MM-dd}\",\"to\":\"{2:yyyy-MM-dd}\"}}",
                        _fieldname,
                        term.RangeFrom,
                        term.RangeTo);
                }
            }
            else if (genericType == typeof (double))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((double) term.RangeTo == default(double))
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1}\"}}", _fieldname, term.RangeFrom);
                }
                else
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1}\",\"to\":\"{2}\"}}", _fieldname, term.RangeFrom,
                        term.RangeTo);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            query.Append("}}");

            return query.ToString();
        }
    }
}