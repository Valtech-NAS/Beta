
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Common.Framework;

    /// <summary>
    /// tests to overload the LabelFor method
    /// </summary>
    [TestFixture]
    public class FormTextForTests : BaseExtensionTests
    {
        /// <summary>
        /// Tests the FormTextFor throws exception when expression is null.
        /// </summary>
        [Test]
        public void TestFormTextForThrowsExceptionWhenExpressionIsNull()
        {
            // Arrange
            var helper = CreateHtmlHelper(new TestModel());
            Expression<Func<TestModel, string>> expression;

            expression = null;

            // Act
            Action s = () => helper.FormTextFor(expression);

            // Assert
            s.ShouldThrow<ArgumentNullException>("Expression can't be null");
        }

        /// <summary>
        /// Tests the FormTextFor throws exception when helper is null.
        /// </summary>
        [Test]
        public void TestFormTextForThrowsExceptionWhenHelperIsNull()
        {
            // Arrange
            var helper = CreateHtmlHelper(new TestModel());
            Expression<Func<TestModel, string>> expression;

            expression = m => m.Value;

            // Act
            Action s = () => HtmlExtensions.FormTextFor(null, expression);

            // Assert
            s.ShouldThrow<ArgumentNullException>("Helper can't be null");
        }

        /// <summary>
        /// Tests the FormTextFor renders property name when no display name assigned.
        /// </summary>
        [Test]
        public void TestFormTextForRendersPropertyNameWhenNoDisplayNameAssigned()
        {
            // Arrange
            var helper = CreateHtmlHelper(new TestModel());
            Expression<Func<TestModel, string>> expression;

            expression = m => m.Value;

            // Act
            var s = helper.FormTextFor(expression);

            // Assert
            Assert.AreEqual("<div class=\"form-group\"><label class=\"form-label\" for=\"Value\">Value</label><input class=\"form-control\" id=\"Value\" name=\"Value\" type=\"text\" value=\"\" /></div>", s.ToString());
        }

        /// <summary>
        /// Tests the display and hint text of the FormTextFor renders.
        /// </summary>
        [Test]
        public void TestFormTextForRendersDisplayNameAndHintText()
        {
            // Arrange
            var helper = CreateHtmlHelper(new TestModel());
            Expression<Func<TestModel, string>> expression;

            expression = m => m.HasAttribute;

            // Act
            var s = helper.FormTextFor(expression);

            // Assert
            Assert.AreEqual(
                "<div class=\"form-group\"><label class=\"form-label\" for=\"HasAttribute\">Keywords</label><span class=\"form-hint\">For example</span><input class=\"form-control\" id=\"HasAttribute\" name=\"HasAttribute\" type=\"text\" value=\"\" /></div>",
                s.ToString());
        }

        /// <summary>
        /// Test model for testing
        /// </summary>
        private class TestModel
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public string Value { get; set; }

            /// <summary>
            /// Gets or sets the has attribute.
            /// </summary>
            /// <value>
            /// The has attribute.
            /// </value>
            [Display(Name = "Keywords", Description = "For example")]
            public string HasAttribute { get; set; }
        }
    }
}