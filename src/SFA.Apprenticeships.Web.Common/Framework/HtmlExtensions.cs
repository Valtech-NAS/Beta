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
        #region Text and TextArea

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
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper, 
                                    expression, 
                                    helper.TextBoxFor, 
                                    labelText, 
                                    hintText, 
                                    false, 
                                    containerHtmlAttributes, 
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes);
        }

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormPasswordFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper,
                                    expression,
                                    helper.PasswordFor,
                                    labelText,
                                    hintText,
                                    false,
                                    containerHtmlAttributes,
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes);
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
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper, 
                                    expression, 
                                    helper.TextAreaFor, 
                                    labelText, 
                                    hintText, 
                                    true, 
                                    containerHtmlAttributes, 
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes);
        }

        private static MvcHtmlString BuildFormControl<TModel, TProperty>(
                    HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    Func<Expression<Func<TModel, TProperty>>, IDictionary<string, object>, MvcHtmlString> controlFunc,
                    string labelText = null,
                    string hintText = null,
                    bool addMaxLengthCounter = false,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var validationError = HasValidationError(helper, expression);
            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            var controlAttributes = MergeAttributes("form-control", controlHtmlAttributes);
            var labelAttributes = MergeAttributes("form-label", labelHtmlAttributes);
            var hintAttributes = MergeAttributes("form-hint", hintHtmlAttributes);

            var validator = helper.ValidationMessageFor(expression, null);

            return FormText(
                helper.LabelFor(expression, labelText, labelAttributes),
                helper.HintFor(expression, hintText, hintAttributes),
                controlFunc(expression, controlAttributes),
                validator,
                AnchorFor(helper, expression),
                addMaxLengthCounter ? CharactersLeftFor(helper, expression) : null,
                addMaxLengthCounter ? ScreenReaderSpan(helper, expression) : null,
                containerAttributes,
                validationError
                );
        }

        private static MvcHtmlString FormText(MvcHtmlString labelContent,
                                    MvcHtmlString hintContent,
                                    MvcHtmlString fieldContent,
                                    MvcHtmlString validationMessage,
                                    MvcHtmlString anchorTag,
                                    MvcHtmlString maxLengthSpan,
                                    MvcHtmlString ariaLimitVisuallyHidden,
                                    RouteValueDictionary containerHtmlAttributes,
                                    bool validationError = false
                                    )
        {
            var container = new TagBuilder("div");
            container.MergeAttributes(containerHtmlAttributes);

            if (validationError)
            {
                container.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            container.InnerHtml += string.Concat(anchorTag, labelContent, hintContent, fieldContent, maxLengthSpan, ariaLimitVisuallyHidden, validationMessage);

            return MvcHtmlString.Create(container.ToString());
        }

        #endregion

        #region Checkbox

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormCheckBoxFor<TModel>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, bool>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            var container = new TagBuilder("div");
            var label = new TagBuilder("label");

            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            var controlAttributes = MergeAttributes("", controlHtmlAttributes);
            var labelAttributes = MergeAttributes("", labelHtmlAttributes);
            //var hintAttributes = MergeAttributes("form-hint", hintHtmlAttributes);
            
            var validationError = HasValidationError(helper, expression);
            var validator = helper.ValidationMessageFor(expression, null);
            var anchorTag = AnchorFor(helper, expression);

            container.MergeAttributes(containerAttributes);

            if (validationError)
            {
                container.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            label.MergeAttributes(labelAttributes);
            label.Attributes.Add("for", helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = helper.CheckBoxFor(expression, controlAttributes).ToString();
            label.InnerHtml += GetDisplayName(helper, expression, labelText);

            container.InnerHtml += string.Concat(anchorTag, label.ToString(), validator);

            return MvcHtmlString.Create(container.ToString());
        }

        #endregion

        #region Radio Buttons

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormRadioButtonsYesNo<TModel>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, bool>> expression,
                    string labelText = null,
                    object containerHtmlAttributes = null,
                    object subContainerHtmlAttributes = null,
                    object labelHtmlAttributes = null)
        {
            var container = new TagBuilder("div");
            var subContainer = new TagBuilder("div");
            var titleLabel = new TagBuilder("p");
            var yesLabel = new TagBuilder("label");
            var noLabel = new TagBuilder("label");

            var value = bool.Parse(helper.ValueFor(expression).ToString());

            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            var subContainerAttributes = MergeAttributes("form-group form-group-compound", subContainerHtmlAttributes);
            var labelAttributes = MergeAttributes("form-label", labelHtmlAttributes);
            
            var yesLabelAttributes = MergeAttributes("block-label" + (value ? " selected" : ""), labelHtmlAttributes);
            var noLabelAttributes = MergeAttributes("block-label" + (!value ? " selected" : ""), labelHtmlAttributes);

            container.MergeAttributes(containerAttributes);
            subContainer.MergeAttributes(subContainerAttributes);
            titleLabel.MergeAttributes(labelAttributes);

            titleLabel.InnerHtml = GetDisplayName(helper, expression, labelText).ToString();

            var fieldId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var yesRadio = helper.RadioButtonFor(expression, true, GetCheckedAttribute(fieldId + "-yes", value, true)) + "Yes";
            var noRadio = helper.RadioButtonFor(expression, false, GetCheckedAttribute(fieldId + "-no", value, false)) + "No";

            yesLabel.MergeAttributes(yesLabelAttributes);
            noLabel.MergeAttributes(noLabelAttributes);

            yesLabel.InnerHtml += yesRadio;
            noLabel.InnerHtml += noRadio;

            subContainer.InnerHtml += string.Format("{0}{1}{2}", titleLabel, yesLabel, noLabel);
            container.InnerHtml += subContainer.ToString();

            return MvcHtmlString.Create(container.ToString());
        }

        private static object GetCheckedAttribute(string id, bool value, bool field)
        {
            if (value == field)
            {
                return new {id, @checked = "checked"};
            }
            return new {id};
        }

        #endregion

        #region Hint, Anchor, Characters Left

        public static MvcHtmlString GetDisplayName<TModel, TProperty>(
                    this HtmlHelper<TModel> htmlHelper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null)
        {
            if (!string.IsNullOrWhiteSpace(labelText))
            {
                return MvcHtmlString.Create(labelText);
            }

            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value);
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

            // TODO: NOTIMPL: This needs to be calculated dyamically either by refelction from from the generated HTML of the control.
            tag.SetInnerText("4000");
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }


        private static MvcHtmlString ScreenReaderSpan<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var tag = new TagBuilder("span");
            tag.Attributes.Add("aria-live", "polite");
            tag.Attributes.Add("class", "visuallyhidden aria-limit");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region Helpers

        private static bool HasValidationError<TModel, TProperty>(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var htmlFieldPrefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            var fullyQualifiedName = string.IsNullOrEmpty(htmlFieldPrefix) ? expressionText : string.Join(".", htmlFieldPrefix, expressionText);
            return !helper.ViewData.ModelState.IsValidField(fullyQualifiedName);
        }

        private static RouteValueDictionary MergeAttributes(string baseClassName, object extendedAttributes)
        {
            var mergeAttributes = extendedAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(extendedAttributes) : new RouteValueDictionary();

            if (mergeAttributes.ContainsKey("class"))
            {
                mergeAttributes["class"] += " " + baseClassName;
            }
            else
            {
                mergeAttributes.Add("class", baseClassName);
            }

            return mergeAttributes;
        }

        #endregion
    }
}