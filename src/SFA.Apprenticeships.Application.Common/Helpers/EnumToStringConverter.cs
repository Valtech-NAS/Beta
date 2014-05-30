namespace SFA.Apprenticeships.Application.Common.Helpers
{
    using System;
    using Newtonsoft.Json;

    public class EnumToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.BaseType == typeof(Enum);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}
