namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using CuttingEdge.Conditions;

    public static class HtmlExtensions
    {
        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormTextFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper, expression, helper.TextBoxFor, labelText, hintText, false, containerHtmlAttributes, controlHtmlAttributes);
        }

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormTextAreaFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper, expression, helper.TextAreaFor, labelText, hintText, true, containerHtmlAttributes, controlHtmlAttributes);
        }

        private static MvcHtmlString BuildFormControl<TModel, TProperty>(
                    HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    Func<Expression<Func<TModel, TProperty>>, IDictionary<string, object>, MvcHtmlString> controlFunc,
                    string labelText = null,
                    string hintText = null,
                    bool addMaxLengthCounter = false,
                    object containerHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            ModelState modelState;
            var validationError = false;
            var name = ExpressionHelper.GetExpressionText(expression);

            if (!string.IsNullOrEmpty(name) && helper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                validationError = modelState.Errors.Count > 0;
            }

            RouteValueDictionary controlAttributes = containerHtmlAttributes != null ? new RouteValueDictionary(controlHtmlAttributes) : new RouteValueDictionary();

            if (controlAttributes.ContainsKey("class"))
            {
                controlAttributes["class"] += " form-control";
            }
            else
            {
                controlAttributes.Add("class", "form-control");
            }

            var validator = helper.ValidationMessageFor(expression, null, new { @class = "hidden" });

            return FormText(
                helper.LabelFor(expression, labelText, new { @class = "form-label" }),
                helper.HintFor(expression, hintText, new { @class = "form-hint" }),
                controlFunc(expression, controlAttributes),
                validator,
                AnchorFor(helper, expression),
                addMaxLengthCounter ? CharactersLeftFor(helper, expression) : null,
                validationError,
                containerHtmlAttributes);
        }

        private static MvcHtmlString FormText(MvcHtmlString labelContent,
                                            MvcHtmlString hintContent,
                                            MvcHtmlString fieldContent,
                                            MvcHtmlString validationMessage,
                                            MvcHtmlString anchorTag,
                                            MvcHtmlString maxLengthSpan,
                                            bool validationError = false, 
                                            object containerHtmlAttributes = null)
        {
            var container = new TagBuilder("div");
            
            if (validationError) { container.AddCssClass(HtmlHelper.ValidationInputCssClassName);             }
            if (containerHtmlAttributes != null) { container.MergeAttributes(new RouteValueDictionary(containerHtmlAttributes)); }
            //Needs added after container.MergeAttributes as MergeAttributes will overwrite existing values.
            container.AddCssClass("form-group");

            container.InnerHtml += string.Concat(anchorTag, labelContent, hintContent, fieldContent, maxLengthSpan, validationMessage);
            
            return MvcHtmlString.Create(container.ToString());
        }

        private static MvcHtmlString HintFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string hintText = null, object htmlAttributes = null)
        {
            return HintFor(helper, expression, hintText, new RouteValueDictionary(htmlAttributes));
        }

        private static MvcHtmlString HintFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string hintText, IDictionary<string, object> htmlAttributes)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var labelText = hintText ?? metadata.Description ?? string.Empty;
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("span");
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString AnchorFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var elementId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(metadata.PropertyName);
            
            if (string.IsNullOrWhiteSpace(elementId))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("a");
            tag.Attributes.Add("name", elementId.ToLower());
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString CharactersLeftFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var tag = new TagBuilder("span");
            tag.Attributes.Add("class", "form-hint maxchar-count");
            
            //TODO: This needs to be calculated dyamically either by refelction from from the generated HTML of the control.
            tag.SetInnerText("4000");
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}