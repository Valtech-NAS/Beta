
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
            object htmlAttributes = null)
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

            return FormText(
                helper.LabelFor(expression, labelText, new { @class = "form-label" }).ToString(),
                helper.HintFor(expression, hintText, new { @class = "form-hint" }).ToString(),
                helper.TextBoxFor(expression, new { @class = "form-control" }).ToString(),
                validationError,
                htmlAttributes);
        }

        private static MvcHtmlString FormText(string labelContent, string hintContent, string fieldContent, bool validationError = false, object htmlAttributes = null)
        {
            var container = new TagBuilder("div");
            container.AddCssClass("form-group");
            if (validationError)
            {
                container.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            if (htmlAttributes != null)
            {
                container.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            container.InnerHtml += labelContent;
            container.InnerHtml += hintContent;
            container.InnerHtml += fieldContent;

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
    }
}