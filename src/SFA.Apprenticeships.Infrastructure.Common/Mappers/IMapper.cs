namespace SFA.Apprenticeships.Infrastructure.Common.Mappers
{
    using System;

    public interface IMapper
    {
        object Map(object source, Type sourceType, Type destinationType);
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}
