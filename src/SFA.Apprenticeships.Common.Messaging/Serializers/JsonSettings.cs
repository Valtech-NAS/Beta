namespace SFA.Apprenticeships.Common.Messaging.Serializers
{
    using ServiceStack.Text;

    internal class JsonSettings
    {
        public static void Initialize()
        {
            JsConfig.ExcludeTypeInfo = false;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig.AlwaysUseUtc = true;
        }
    }
}
