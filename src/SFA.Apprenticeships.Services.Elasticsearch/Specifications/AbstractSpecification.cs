namespace SFA.Apprenticeships.Services.Elasticsearch.Specifications
{
    using System;
    using System.Linq.Expressions;
    using SFA.Apprenticeships.Services.Elasticsearch.Interfaces;

    public abstract class AbstractSpecification<TModel, TProperty> : ISpecification<TModel>
    {
        protected AbstractSpecification(Expression<Func<TModel, TProperty>> fieldname)
        {
            if (fieldname != null)
            {
                var memberExpression = fieldname.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentNullException("fieldname");
                }

                Fieldname = memberExpression.Member.Name;
                Term = fieldname.Compile();
            }
        }

        protected string Fieldname { get; private set; }
        protected Func<TModel, TProperty> Term { get; private set; }
        public abstract string Build(TModel entity);
    }
}
