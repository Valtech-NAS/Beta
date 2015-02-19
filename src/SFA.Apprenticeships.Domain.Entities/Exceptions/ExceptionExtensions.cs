namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    public static class ExceptionExtensions
    {
        public static void AddData(this Exception e, object data)
        {
            if (data == null) return;

            foreach (var property in data.GetType().GetProperties())
            {
                e.Data[property.Name] = property.GetValue(data) ?? "<null>";
            }
        }
    }
}
