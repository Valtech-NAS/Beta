namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Serializers
{
    using ServiceStack.Text;

    internal class JsonSettings
    {
        public static void Initialize()
        {
            JsConfig.ExcludeTypeInfo = false;
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.AlwaysUseUtc = true;
        }
    }
}
