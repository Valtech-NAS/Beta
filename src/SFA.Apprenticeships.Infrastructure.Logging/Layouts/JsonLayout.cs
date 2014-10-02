namespace SFA.Apprenticeships.Infrastructure.Logging.Layouts
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
        [ArrayParameter(typeof (JsonProperty), "property")]
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

            for (var i = 1; i <= Properties.Count; i++)
            {
                var prop = Properties[i - 1];
                var text = prop.Layout.Render(logEvent);

                if (string.IsNullOrWhiteSpace(text)) continue;

                var detail = Json.Encode(text.Replace("{", "[").Replace("}", "]"));

                sb.Append(string.Format("\"{0}\" : {1},", prop.Name, detail));
            }

            var json = sb.ToString();
            return json.Substring(0, json.Length - 1) + "}";
        }
    }
}
