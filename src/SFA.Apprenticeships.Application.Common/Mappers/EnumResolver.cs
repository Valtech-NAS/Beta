
namespace SFA.Apprenticeships.Application.Common.Mappers
{
    using System;
    using AutoMapper;
    using SFA.Apprenticeships.Application.Common.Helpers;

    public class EnumResolver<T> : ValueResolver<string, T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected override T ResolveCore(string source)
        {
            T result;

            EnumExtensions.TryGetEnumFromDescription(source, out result);
            return result;
        }
    }
}
