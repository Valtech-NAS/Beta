namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using Common.Mappers;
    using Domain.Entities.Vacancy;
    using VacancyDetailProxy;

    public class VacancyDetailMapper : MapperEngine
    {
        public override void Initialize()
        {
            //todo: write mapper (this is just a copy from summary entity mapper)
            Mapper.CreateMap<VacancyFullData, VacancyDetail>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VacancyReference))
                .ForMember(d => d.AddressLine1, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine1))
                .ForMember(d => d.AddressLine2, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine2))
                .ForMember(d => d.AddressLine3, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine3))
                .ForMember(d => d.AddressLine4, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine4))
                .ForMember(d => d.AddressLine5, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine5))
                .ForMember(d => d.PostCode, opt => opt.MapFrom(src => src.VacancyAddress.PostCode))
                .ForMember(d => d.Town, opt => opt.MapFrom(src => src.VacancyAddress.Town))
                .ForMember(d => d.County, opt => opt.MapFrom(src => src.VacancyAddress.County))
                .ForMember(d => d.LocalAuthority, opt => opt.MapFrom(src => src.VacancyAddress.LocalAuthority))
                .ForMember(d => d.Created, opt => opt.MapFrom(src => src.CreatedDateTime))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.Framework, opt => opt.MapFrom(src => src.ApprenticeshipFramework))
                .ForMember(d => d.Location, opt => opt.ResolveUsing<VacancyDetailAddressResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.ProviderName, opt => opt.MapFrom(src => src.LearningProviderName))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle))
                .ForMember(d => d.VacancyLocationType, opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.VacancyType, opt => opt.ResolveUsing<VacancyTypeResolver>().FromMember(src => src.VacancyType))
                .ForMember(d => d.WageType, opt => opt.MapFrom(src => src.WageType));

            //Mapper.CreateMap<VacancyFullData, VacancyDetail>().ConvertUsing<DetailDataConverter>();
        }
    }

    class DetailDataConverter : ITypeConverter<VacancyFullData, VacancyDetail>
    {
        public VacancyDetail Convert(ResolutionContext context)
        {
            return context.Engine.Map<VacancyFullData, VacancyDetail>((VacancyFullData)context.SourceValue);
        }
    }
}
