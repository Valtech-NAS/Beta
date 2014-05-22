using System;
using AutoMapper;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers
{
    public class EnumResolver<T> : ValueResolver<string, T> where T : struct
    {
        protected override T ResolveCore(string source)
        {
            T result;

            Enum.TryParse(source, true, out result);
            return result;
        }
    }
}
