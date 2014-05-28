using System;

namespace SFA.Apprenticeships.Common.Interfaces.Mapper
{
    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}
