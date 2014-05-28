namespace SFA.Apprenticeships.Common.Logging.Layouts
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Helpers;
    using NLog;
    using NLog.Config;
    using NLog.Layouts;

    /// <summary>
    /// A specialized layout that renders Json-formatted events.
    /// </summary>
    [Layout("JsonLayout")]
    [ThreadAgnostic]
    [AppDomainFixedOutput]
    public class JsonLayout : Layout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLayout"/> class.
        /// </summary>
        public JsonLayout()
        {
            Properties = new List<JsonProperty>();
        }

        /// <summary>
        /// Gets the array of parameters to be passed.
        /// </summary>
        /// <docgen category='Json Property' order='10' />
        [ArrayParameter(typeof(JsonProperty), "property")]
        public IList<JsonProperty> Properties { get; private set; }

        /// <summary>
        /// Formats the log event for write.
        /// </summary>
        /// <param name="logEvent">The log event to be formatted.</param>
        /// <returns>A string representation of the log event.</returns>
        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            var sb = new StringBuilder();

            sb.Append("{");

            for (int i = 1; i <= Properties.Count; i++)
            {
                JsonProperty prop = Properties[i - 1];
                string text = prop.Layout.Render(logEvent);

                if (!string.IsNullOrEmpty(text))
                {
                    sb.Append(string.Format("\"{0}\" : ", prop.Name));
                    sb.Append(string.Format(Json.Encode(text)));
                    sb.Append(",");
                }
            }

            var json = sb.ToString();
            return json.Substring(0, json.Length-1) + "}";
        }
    }
}