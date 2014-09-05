namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using Common.Mappers;
    using VacancyDetailProxy;

    public class VacancyDetailMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancyFullData, Domain.Entities.Vacancies.VacancyDetail>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VacancyReference))
                .ForMember(d => d.LocalAuthority, opt => opt.MapFrom(src => src.VacancyAddress.LocalAuthority))
                .ForMember(d => d.Created, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.PossibleStartDate.ToUniversalTime()))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.Framework, opt => opt.MapFrom(src => src.ApprenticeshipFramework))
                .ForMember(d => d.VacancyAddress,
                    opt => opt.ResolveUsing<VacancyDetailAddressResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.ProviderName, opt => opt.MapFrom(src => src.LearningProviderName))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle))
                .ForMember(d => d.VacancyLocationType,
                    opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.VacancyType,
                    opt => opt.ResolveUsing<VacancyTypeResolver>().FromMember(src => src.VacancyType))
                .ForMember(d => d.WageDescription, opt => opt.MapFrom(src => src.WageText))
                .ForMember(d => d.OtherInformation, opt => opt.MapFrom(src => src.OtherImportantInformation))
                .ForMember(d => d.ProviderDescription, opt => opt.MapFrom(src => src.LearningProviderDesc))
                .ForMember(d => d.Contact, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(d => d.ProviderSectorPassRate, opt => opt.MapFrom(src => src.LearningProviderSectorPassRate))
                .ForMember(d => d.InterviewFromDate, opt => opt.MapFrom(src => src.InterviewFromDate.ToUniversalTime()))
                .ForMember(d => d.ClosingDate, opt => opt.MapFrom(src => src.ClosingDate.ToUniversalTime()))
                // TODO: US484: need to map when field is available in NAS Gateway interface.
                .ForMember(d => d.IsEmployerAnonymous, opt => opt.MapFrom(src => false))
                .ForMember(d => d.AnonymousEmployerName, opt => opt.MapFrom(src => src.EmployerDescription))
                .ForMember(d => d.ApplyViaEmployerWebsite, opt => opt.Ignore())
                .ForMember(d => d.IsRecruitmentAgencyAnonymous, opt => opt.Ignore())
                .ForMember(d => d.TradingName, opt => opt.Ignore())
                .ForMember(d => d.IsNasProvider, opt => opt.Ignore())
                .ForMember(d => d.RealityCheck, opt => opt.Ignore());
        }
    }
}
