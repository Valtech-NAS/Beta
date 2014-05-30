namespace SFA.Apprenticeships.Infrastructure.Postcode.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Domain.Entities.Postcode;

    public class PostcodeMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<Entities.PostcodeInfo, PostcodeInfo>()
                .ForMember(d => d.AdminDistrict, opt => opt.MapFrom(src => src.Admin_District))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(d => d.Postcode, opt => opt.MapFrom(src => src.Postcode));

            Mapper.CreateMap<IList<Entities.PostcodeInfo>, IList<PostcodeInfo>>().ConvertUsing<PostcodesConverter>();
        }
    }

    class PostcodesConverter : ITypeConverter<IList<Entities.PostcodeInfo>, IList<PostcodeInfo>>
    {
        public IList<PostcodeInfo> Convert(ResolutionContext context)
        {
            return
                (from item in (Entities.PostcodeInfo[])context.SourceValue
                select context.Engine.Map<Entities.PostcodeInfo, PostcodeInfo>(item)).ToList();
        }
    }
}