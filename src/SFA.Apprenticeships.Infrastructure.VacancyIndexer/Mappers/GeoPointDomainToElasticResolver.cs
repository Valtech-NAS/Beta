namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using AutoMapper;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;

    public class GeoPointDomainToElasticResolver : ValueResolver<Domain.Entities.Location.GeoPoint, Elastic.Common.Entities.GeoPoint>
    {
        protected override GeoPoint ResolveCore(Domain.Entities.Location.GeoPoint source)
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
