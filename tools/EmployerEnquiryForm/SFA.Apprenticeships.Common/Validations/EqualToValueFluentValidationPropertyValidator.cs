/*
 * This is a fix to support client side validation of checkboxes being 
 * specific values, see links below for more details
 * http://stackoverflow.com/questions/9808794/validate-checkbox-on-the-client-with-fluentvalidation-mvc-3
 * http://pastebin.com/7uzUJz71
 */
namespace SFA.Apprenticeships.Web.Common.Validations
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using FluentValidation.Internal;
    using FluentValidation.Mvc;
    using FluentValidation.Validators;

    public class EqualToValueFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        public EqualToValueFluentValidationPropertyValidator(ModelMetadata metadata,
            ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
            : base(metadata, controllerContext, rule, validator)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules())
            {
                yield break;
            }
            var validator = (EqualValidator) Validator;

            string errorMessage = new MessageFormatter()
                .AppendPropertyName(Rule.GetDisplayName())
                .AppendArgument("ValueToCompare", validator.ValueToCompare)
                .BuildMessage(validator.ErrorMessageSource.GetString());

            var rule = new ModelClientValidationRule {ErrorMessage = errorMessage, ValidationType = "equaltovalue"};
            rule.ValidationParameters["valuetocompare"] = validator.ValueToCompare;
            yield return rule;
        }
    }
}