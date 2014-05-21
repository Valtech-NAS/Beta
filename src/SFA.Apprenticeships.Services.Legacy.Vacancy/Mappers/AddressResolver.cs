using AutoMapper;
using SFA.Apprenticeships.Common.Entities.Elasticsearch;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers
{
    public class AddressResolver : ValueResolver<AddressData, IGeoPoint>
    {
        protected override IGeoPoint ResolveCore(AddressData source)
        {
            var point = new GeoPoint
            {
                lat = (double) source.Latitude.GetValueOrDefault(),
                lon = (double) source.Longitude.GetValueOrDefault()
            };

            return point;
        }
    }
}
