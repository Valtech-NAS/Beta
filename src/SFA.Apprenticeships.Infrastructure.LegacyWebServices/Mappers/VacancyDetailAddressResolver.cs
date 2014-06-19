namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using VacancyDetailProxy;
    using Domain.Entities.Location;

    public class VacancyDetailAddressResolver : ValueResolver<AddressData, GeoPoint>
    {
        protected override GeoPoint ResolveCore(AddressData source)
        {
            var point = new GeoPoint();

            if (source != null)
            {
                point.Latitute = (double) source.Latitude.GetValueOrDefault();
                point.Longitude = (double) source.Longitude.GetValueOrDefault();
            }

            return point;
        }
    }
}
