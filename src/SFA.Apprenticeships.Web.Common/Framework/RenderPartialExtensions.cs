namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.UI;

    public static class RenderPartialExtensions
    {
        public static string RenderPartialToString(this ControllerContext context, string partialViewName, object model)
        {
            return RenderPartialToStringMethod(context, partialViewName, model);
        }

        public static string RenderPartialToString(ControllerContext context, string partialViewName,
            ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            return RenderPartialToStringMethod(context, partialViewName, viewData, tempData);
        }

        private static string RenderPartialToStringMethod(ControllerContext context, string partialViewName,
            ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            var result = ViewEngines.Engines.FindPartialView(context, partialViewName);

            if (result.View != null)
            {
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    using (var output = new HtmlTextWriter(sw))
                    {
                        var viewContext = new ViewContext(context, result.View, viewData, tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }

        private static string RenderPartialToStringMethod(ControllerContext context, string partialViewName,
            object model)
        {
            return RenderPartialToStringMethod(context, partialViewName, new ViewDataDictionary(model),
                new TempDataDictionary());
        }

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
            ModelState modelState;
            var validationError = false;
            var name = ExpressionHelper.GetExpressionText(expression);

            if (!string.IsNullOrEmpty(name) && helper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                validationError = modelState.Errors.Count > 0;
            }

            return FormText(
                helper.LabelFor(expression, labelText, new {@class = "form-label"}).ToString(),
                helper.HintFor(expression, hintText, new { @class = "form-hint" }).ToString(),
                helper.TextBoxFor(expression, new {@class = "form-control"}).ToString(),
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

        /// <summary>
        /// Creates the hint element within the form control.
        /// </summary>
        /// <returns>The Html to render.</returns>
        public static MvcHtmlString HintFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, 
            string hintText = null,
            object htmlAttributes = null)
        {
            return HintFor(html, expression, hintText, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Creates the hint element within the form control.
        /// </summary>
        /// <returns>The Html to render.</returns>
        public static MvcHtmlString HintFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, 
            string hintText,
            IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
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