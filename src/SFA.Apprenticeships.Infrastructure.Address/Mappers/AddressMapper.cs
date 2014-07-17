namespace SFA.Apprenticeships.Infrastructure.Address.Mappers
{
    using AutoMapper;
    using Common.Mappers;

    public class AddressMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Elastic.Common.Entities.Address, Domain.Entities.Locations.Address>()
                .ForMember(d => d.GeoPoint,
                    opt => opt.ResolveUsing<ElasticAddressToGeoPointDomainResolver>().FromMember(src => src));
        }
    }

    public class ElasticAddressToGeoPointDomainResolver : ValueResolver<Elastic.Common.Entities.Address, Domain.Entities.Locations.GeoPoint>
    {
        protected override Domain.Entities.Locations.GeoPoint ResolveCore(Elastic.Common.Entities.Address source)
        {
            var point = new Domain.Entities.Locations.GeoPoint();

            if (source != null)
            {
                point.Latitude = source.Latitude;
                point.Longitude = source.Longitude;
            }

            return point;
        }
    }
}
