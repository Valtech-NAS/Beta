namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using Common.Mappers;
    using VacancyDetailProxy;

    public class VacancyDetailMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancyFullData, Domain.Entities.Vacancies.VacancyDetail>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VacancyReference))
                .ForMember(d => d.LocalAuthority, opt => opt.MapFrom(src => src.VacancyAddress.LocalAuthority))
                .ForMember(d => d.Created, opt => opt.MapFrom(src => src.CreatedDateTime))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.PossibleStartDate))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.Framework, opt => opt.MapFrom(src => src.ApprenticeshipFramework))
                .ForMember(d => d.VacancyAddress, opt => opt.ResolveUsing<VacancyDetailAddressResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.ProviderName, opt => opt.MapFrom(src => src.LearningProviderName))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle))
                .ForMember(d => d.VacancyLocationType, opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.VacancyType, opt => opt.ResolveUsing<VacancyTypeResolver>().FromMember(src => src.VacancyType))
                .ForMember(d => d.WageDescription, opt => opt.MapFrom(src => src.WageText))
                .ForMember(d => d.OtherInformation, opt => opt.MapFrom(src => src.OtherImportantInformation))
                .ForMember(d => d.ProviderDescription, opt => opt.MapFrom(src => src.LearningProviderDesc))
                .ForMember(d => d.Contact, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(d => d.ProviderSectorPassRate, opt => opt.MapFrom(src => src.LearningProviderSectorPassRate));
        }
    }
}
