
namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers
{
    using System;
    using AutoMapper;
    using SFA.Apprenticeships.Common.Helpers;

    public class EnumResolver<T> : ValueResolver<string, T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected override T ResolveCore(string source)
        {
            T result;

            ExtensionMethods.TryGetEnumFromDescription(source, out result);
            return result;
        }
    }
}
