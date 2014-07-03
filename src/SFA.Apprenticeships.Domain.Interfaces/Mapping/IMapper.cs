namespace SFA.Apprenticeships.Domain.Interfaces.Mapping
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource sourceObject);
    }
}