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
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            ModelState modelState;
            var validationError = false;
            var name = ExpressionHelper.GetExpressionText(expression);
            RouteValueDictionary controlAttributes = containerHtmlAttributes != null ? new RouteValueDictionary(controlHtmlAttributes) : new RouteValueDictionary();
           
            if (controlAttributes.ContainsKey("class")){
                controlAttributes["class"] += " form-control";
            }else{
                controlAttributes.Add("class", "form-control");
            }

            if (!string.IsNullOrEmpty(name) && helper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                validationError = modelState.Errors.Count > 0;
            }

            var validator = helper.ValidationMessageFor(expression, null, new {@class = "hidden"});

            return FormText(
                helper.LabelFor(expression, labelText, new {@class = "form-label"}).ToString(),
                helper.HintFor(expression, hintText, new {@class = "form-hint"}).ToString(),
                helper.TextBoxFor(expression, controlAttributes).ToString(),
                validator == null ? "" : validator.ToString(),
                AnchorFor(helper, expression).ToString(),
                string.Empty,
                validationError,
                containerHtmlAttributes);
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
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            ModelState modelState;
            var validationError = false;
            var name = ExpressionHelper.GetExpressionText(expression);

            RouteValueDictionary controlAttributes = containerHtmlAttributes != null ? new RouteValueDictionary(controlHtmlAttributes) : new RouteValueDictionary();

            if (controlAttributes.ContainsKey("class")){
                controlAttributes["class"] += " form-control";
            }else{
                controlAttributes.Add("class", "form-control");
            }

            if (!string.IsNullOrEmpty(name) && helper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                validationError = modelState.Errors.Count > 0;
            }

            var validator = helper.ValidationMessageFor(expression, null, new { @class = "hidden" });

            return FormText(
                helper.LabelFor(expression, labelText, new { @class = "form-label" }).ToString(),
                helper.HintFor(expression, hintText, new { @class = "form-hint" }).ToString(),
                helper.TextAreaFor(expression, controlAttributes).ToString(),
                validator == null ? "" : validator.ToString(),
                AnchorFor(helper, expression).ToString(),
                CharactersLeftFor(helper, expression).ToString(),
                validationError,
                containerHtmlAttributes);
        }

        private static MvcHtmlString FormText(string labelContent, 
                                            string hintContent, 
                                            string fieldContent,
                                            string validationMessage,
                                            string anchorTag,
                                            string maxLengthSpan,
                                            bool validationError = false, 
                                            object containerHtmlAttributes = null)
        {
            var container = new TagBuilder("div");
            
            if (validationError)
            {
                container.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            if (containerHtmlAttributes != null)
            {
                container.MergeAttributes(new RouteValueDictionary(containerHtmlAttributes));
            }
            container.AddCssClass("form-group");

            container.InnerHtml += string.Concat(anchorTag, 
                                                labelContent, 
                                                hintContent, 
                                                fieldContent, 
                                                validationMessage,
                                                maxLengthSpan);
            
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