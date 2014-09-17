namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class SpanLabelExtensions
    {
        public static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return SpanFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("span");
            tag.MergeAttributes(htmlAttributes);
            tag.SetInnerText(labelText);
            //tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}