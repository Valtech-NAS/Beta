namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common.Mapping;
    using Domain.Entities.Vacancy;
    using VacancySummaryProxy;

    public class VacancySummaryMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummaryData, VacancySummary>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => (long)src.VacancyReference))
                //.ForMember(d => d.UpdateReference, opt => opt.Ignore())
                .ForMember(d => d.AddressLine1, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine1))
                .ForMember(d => d.AddressLine2, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine2))
                .ForMember(d => d.AddressLine3, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine3))
                .ForMember(d => d.AddressLine4, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine4))
                .ForMember(d => d.AddressLine5, opt => opt.MapFrom(src => src.VacancyAddress.AddressLine5))
                .ForMember(d => d.ClosingDate, opt => opt.MapFrom(src => src.ClosingDate))
                .ForMember(d => d.County, opt => opt.MapFrom(src => src.VacancyAddress.County))
                .ForMember(d => d.Created, opt => opt.MapFrom(src => src.CreatedDateTime))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.EmployerName, opt => opt.MapFrom(src => src.EmployerName))
                .ForMember(d => d.Framework, opt => opt.MapFrom(src => src.ApprenticeshipFramework))
                .ForMember(d => d.LocalAuthority, opt => opt.MapFrom(src => src.VacancyAddress.LocalAuthority))
                .ForMember(d => d.Location, opt => opt.ResolveUsing<AddressResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.NumberOfPositions, opt => opt.MapFrom(src => src.NumberOfPositions))
                .ForMember(d => d.PostCode, opt => opt.MapFrom(src => src.VacancyAddress.PostCode))
                .ForMember(d => d.ProviderName, opt => opt.MapFrom(src => src.LearningProviderName))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle))
                .ForMember(d => d.Town, opt => opt.MapFrom(src => src.VacancyAddress.Town))
                .ForMember(d => d.TypeOfLocation, opt => opt.ResolveUsing<EnumResolver<VacancyLocationType>>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.TypeOfVacancy, opt => opt.ResolveUsing<EnumResolver<VacancyType>>().FromMember(src => src.VacancyType))
                .ForMember(d => d.VacancyUrl, opt => opt.MapFrom(src => src.VacancyUrl));

            Mapper.CreateMap<VacancySummaryData[], IEnumerable<VacancySummary>>().ConvertUsing<SummaryDataConverter>();
        }
    }

    class SummaryDataConverter : ITypeConverter<VacancySummaryData[], IEnumerable<VacancySummary>>
    {
        public IEnumerable<VacancySummary> Convert(ResolutionContext context)
        {
            return
                from item in (VacancySummaryData[])context.SourceValue
                select context.Engine.Map<VacancySummaryData, VacancySummary>(item);
        }
    }
}