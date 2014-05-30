namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper
{
    using AutoMapper;
    using SFA.Apprenticeships.Domain.Entities.Location;

    public class GeoPointResolver : ValueResolver<GeoPoint, Elasticsearch.Entities.GeoPoint>
    {
        protected override Elasticsearch.Entities.GeoPoint ResolveCore(GeoPoint source)
        {
            var point = new Elasticsearch.Entities.GeoPoint();

            if (source != null)
            {
                point.lat = source.Latitude;
                point.lon = source.Longitude;
            }

            return point;
        }
    }
}
