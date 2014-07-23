namespace SFA.Apprenticeships.Infrastructure.Logging.Layouts
{
    using NLog.Config;
    using NLog.Layouts;

    /// <summary>
    /// A column in the CSV.
    /// </summary>
    [NLogConfigurationItem]
    [ThreadAgnostic]
    public class JsonProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonProperty" /> class.
        /// </summary>
        public JsonProperty()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonProperty" /> class.
        /// </summary>
        /// <param name="name">The name of property.</param>
        /// <param name="layout">The layout of the property.</param>
        public JsonProperty(string name, Layout layout)
        {
            Name = name;
            Layout = layout;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout of the property.
        /// </summary>
        [RequiredParameter]
        public Layout Layout { get; set; }
    }
}
