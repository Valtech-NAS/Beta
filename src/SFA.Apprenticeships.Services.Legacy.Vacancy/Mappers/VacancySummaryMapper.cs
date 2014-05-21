using System;
using AutoMapper;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.EntityMappers;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers
{
    public class VacancySummaryMapper : MapperEngine
    {
        private readonly IReferenceDataService _service;

        public VacancySummaryMapper(IReferenceDataService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
        }

        public void Initialize()
        {
            Mapper.CreateMap<VacancySummaryData, VacancySummary>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => (long)src.VacancyReference))
                .ForMember(d => d.UpdateReference, opt => opt.UseValue(Guid.NewGuid())) //TODO:: needs to be the message guid...
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
                .ForMember(d => d.Framework, opt => opt.ResolveUsing<FrameworkResolver>()
                    .ConstructedBy(() => new FrameworkResolver(_service))
                    .FromMember(src => src.ApprenticeshipFramework))
                .ForMember(d => d.LocalAuthority, opt => opt.MapFrom(src => src.VacancyAddress.LocalAuthority))
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<AddressResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.NumberOfPositions, opt => opt.MapFrom(src => src.NumberOfPositions))
                .ForMember(d => d.PostCode, opt => opt.MapFrom(src => src.VacancyAddress.PostCode))
                .ForMember(d => d.ProviderName, opt => opt.MapFrom(src => src.LearningProviderName))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle))
                .ForMember(d => d.Town, opt => opt.MapFrom(src => src.VacancyAddress.Town))
                .ForMember(d => d.TypeOfLocation, opt => opt.ResolveUsing(src => (VacancyLocationType) Enum.Parse(typeof (VacancyLocationType), src.VacancyLocationType)))
                .ForMember(d => d.TypeOfVacancy, opt => opt.ResolveUsing(src => (VacancyType) Enum.Parse(typeof (VacancyType), src.VacancyType)))
                .ForMember(d => d.VacancyUrl, opt => opt.MapFrom(src => src.VacancyUrl))
                ;
            
            //.AfterMap((s, d) => d.UpdateReference = Guid.NewGuid()); 
        }
    }
}