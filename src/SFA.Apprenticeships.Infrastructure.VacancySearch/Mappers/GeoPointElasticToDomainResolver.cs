namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Mappers
{
    using AutoMapper;
    using Elastic.Common.Entities;

    public class GeoPointElasticToDomainResolver : ValueResolver<GeoPoint, Domain.Entities.Locations.GeoPoint>
    {
        protected override Domain.Entities.Locations.GeoPoint ResolveCore(GeoPoint source)
        {
            var point = new Domain.Entities.Locations.GeoPoint();

            if (source != null)
            {
                point.Latitude = source.lat;
                point.Longitude = source.lon;
            }

            return point;
        }
    }
}
