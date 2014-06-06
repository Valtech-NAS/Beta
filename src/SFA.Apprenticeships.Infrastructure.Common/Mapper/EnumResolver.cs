namespace SFA.Apprenticeships.Infrastructure.Common.Mapper
{
    using System;
    using AutoMapper;

    public class EnumResolver<T> : ValueResolver<string, T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected override T ResolveCore(string source)
        {
            var result = (T)Enum.Parse(typeof (T), source);
            return result;
        }
    }
}
