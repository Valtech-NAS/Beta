

namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Specifications
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Interfaces;

    public class RangeSpecification<TModel> : AbstractSpecification<TModel, ISortableRange>, IConstraintSpecification<TModel>
    {
        public RangeSpecification(Expression<Func<TModel, ISortableRange>> fieldname)
            : base(fieldname)
        {
        }

        public override string Build(TModel parameters)
        {
            var term = Term.Invoke(parameters);
            if (term == null || !term.HasValue)
            {
                return string.Empty;
            }

            var query = new StringBuilder("{\"range\":{");

            var genericType = term.GetType().UnderlyingSystemType.GenericTypeArguments[0];
            if (genericType == typeof (DateTime))
            {
                if ((DateTime) term.RangeTo == default(DateTime))
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1:yyyy-MM-dd}\"}}", Fieldname, term.RangeFrom);
                }
                else
                {
                    query.AppendFormat(
                        "\"{0}\":{{\"from\":\"{1:yyyy-MM-dd}\",\"to\":\"{2:yyyy-MM-dd}\"}}",
                        Fieldname,
                        term.RangeFrom,
                        term.RangeTo);
                }
            }
            else if (genericType == typeof (double))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((double) term.RangeTo == default(double))
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1}\"}}", Fieldname, term.RangeFrom);
                }
                else
                {
                    query.AppendFormat("\"{0}\":{{\"from\":\"{1}\",\"to\":\"{2}\"}}", Fieldname, term.RangeFrom,
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