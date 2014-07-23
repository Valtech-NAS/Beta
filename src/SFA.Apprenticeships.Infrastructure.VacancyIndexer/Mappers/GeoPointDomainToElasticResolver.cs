namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using AutoMapper;
    using Elastic.Common.Entities;

    public class GeoPointDomainToElasticResolver : ValueResolver<Domain.Entities.Locations.GeoPoint, GeoPoint>
    {
        protected override GeoPoint ResolveCore(Domain.Entities.Locations.GeoPoint source)
        {
            var point = new GeoPoint();

            if (source != null)
            {
                point.lat = source.Latitude;
                point.lon = source.Longitude;
            }

            return point;
        }
    }
}
