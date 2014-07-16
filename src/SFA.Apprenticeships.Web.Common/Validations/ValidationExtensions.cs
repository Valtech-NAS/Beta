namespace SFA.Apprenticeships.Web.Common.Validations
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using FluentValidation;
    using FluentValidation.Mvc;

    public static class ValidationExtensions
    {
        public static ModelStateDictionary ValidateModel<TModel, TValidator>(
            this TModel model, 
            TValidator validator,
            ModelStateDictionary modelState)
            where TModel : class
            where TValidator : IValidator
        {
            var result = validator.Validate(model);

            result.AddToModelState(modelState, string.Empty);

            return modelState;
        }

        /// <summary>
        ///  TODO: DONTKNOW: This method added to overcome the issue in FluentValidation MVC5 implementation. 
        /// Once fixed the above method can be re-instated.
        /// </summary>
        /// <typeparam name="TModel">The parent model</typeparam>
        /// <typeparam name="TProperty">The model to validate</typeparam>
        /// <typeparam name="TValidator">The validator to use</typeparam>
        /// <param name="model">The parent model</param>
        /// <param name="expression">The property of the parent model to validate</param>
        /// <param name="validator">The validator to use</param>
        /// <param name="modelState">The current model state</param>
        /// <returns>The updated model state</returns>
        public static ModelStateDictionary ValidateModel<TModel, TProperty, TValidator>(this TModel model,
            Expression<Func<TModel, TProperty>> expression, TValidator validator, ModelStateDictionary modelState)
            where TModel : class
            where TValidator : IValidator
        {
            var property = expression.Body as MemberExpression;
            if (property != null)
            {
                var result = validator.Validate(expression.Compile().Invoke(model));

                result.AddToModelState(modelState, property.Member.Name);
            }

            return modelState;
        }
    }
}
