namespace SFA.Apprenticeships.Infrastructure.Common.Mapper
{
    using System;
    using AutoMapper;
    using SFA.Apprenticeships.Infrastructure.Common.Helpers;

    public class EnumDescriptionResolver<T> : ValueResolver<string, T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected override T ResolveCore(string source)
        {
            T result;
            EnumExtensions.TryGetEnumFromDescription(source, out result);
            return result;
        }
    }
}
