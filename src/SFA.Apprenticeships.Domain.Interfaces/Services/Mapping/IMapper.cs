using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Services.Mapping
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}
