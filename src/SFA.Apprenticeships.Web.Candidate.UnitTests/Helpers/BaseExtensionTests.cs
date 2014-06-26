namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Helpers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Base abstract for HTMLExtension tests
    /// </summary>
    public abstract class BaseExtensionTests
    {
        /// <summary>
        /// Gets or sets the HTML helper.
        /// </summary>
        /// <value>
        /// The HTML helper.
        /// </value>
        protected HtmlHelper HtmlHelper { get; set; }

        /// <summary>
        /// Creates the HTML helper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model to be assigned as the view data.</param>
        /// <returns>A typed HtmlHelper with fake context and data container</returns>
        protected static HtmlHelper<TModel> CreateHtmlHelper<TModel>(TModel model)
        {
            // Create a ViewContext and assign a fake HttpContext
            var vc = new ViewContext { HttpContext = new FakeHttpContext() };

            // Generate the HtmlHelper with our ViewContext
            var hh = new HtmlHelper<TModel>(vc, new FakeViewDataContainer());

            // Some model assignment stuff...
            hh.ViewData.Model = model;
            vc.ViewData = new ViewDataDictionary(model);

            return hh;
        }

        /// <summary>
        /// A Fake Http Context
        /// </summary>
        protected class FakeHttpContext : HttpContextBase
        {
            /// <summary>
            /// private instance of context items
            /// </summary>
            private readonly Dictionary<object, object> items = new Dictionary<object, object>();

            /// <summary>
            /// When overridden in a derived class, gets a key/value collection that can be used to organize and share data between a module and a handler during an HTTP request.
            /// </summary>
            /// <returns>A key/value collection that provides access to an individual value in the collection by using a specified key.</returns>
            public override IDictionary Items
            {
                get
                {
                    return this.items;
                }
            }
        }

        /// <summary>
        /// A Fake ViewData Container
        /// </summary>
        protected class FakeViewDataContainer : IViewDataContainer
        {
            /// <summary>
            /// private instance of our viewData dictionary
            /// </summary>
            private ViewDataDictionary viewData = new ViewDataDictionary();

            /// <summary>
            /// Gets or sets the view data dictionary.
            /// </summary>
            /// <returns>The view data dictionary.</returns>
            public ViewDataDictionary ViewData
            {
                get { return this.viewData; }
                set { this.viewData = value; }
            }
        }
    }
}

