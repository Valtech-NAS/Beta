namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using Common.Mappers;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class LegacyApprenticeshipVacancyDetailMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GatewayServiceProxy.Vacancy, ApprenticeshipVacancyDetail>()
                .BeforeMap<ApprenticeshipTypeCheck>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                .ForMember(dest => dest.VacancyReference,
                    opt => opt.MapFrom(src => "VAC" + src.VacancyReference.ToString("D9")))

                .ForMember(x => x.VacancyStatus,
                    opt => opt.ResolveUsing<VacancyStatusResolver>().FromMember(src => src.Status))

                .ForMember(dest => dest.ApplicationInstructions,
                    opt => opt.MapFrom(src => src.ApplicationInstructions))

                .ForMember(dest => dest.ApplyViaEmployerWebsite,
                    opt => opt.MapFrom(src => src.ApplyViaEmployerWebsiteSpecified && src.ApplyViaEmployerWebsite))

                .ForMember(dest => dest.Framework,
                    opt => opt.MapFrom(src => src.ApprenticeshipFramework.Description))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.Contact,
                    opt => opt.MapFrom(src => src.ContactForCandidate))

                .ForMember(dest => dest.ProviderName,
                    opt => opt.MapFrom(src => src.ContractedProviderName))

                .ForMember(dest => dest.ContractOwner,
                    opt => opt.MapFrom(src => src.ContractOwner))

                .ForMember(dest => dest.DeliveryOrganisation,
                    opt => opt.MapFrom(src => src.DeliveryOrganisation))

                .ForMember(dest => dest.IsEmployerAnonymous,
                    opt => opt.MapFrom(src => src.EmployerAnonymousSpecified && src.EmployerAnonymous))

                .ForMember(dest => dest.AnonymousEmployerName,
                    opt => opt.MapFrom(src => src.EmployerAnonymousDescription))

                .ForMember(dest => dest.EmployerDescription,
                    opt => opt.MapFrom(src => src.EmployerDescription))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.EmployerWebsite,
                    opt => opt.MapFrom(src => src.EmployerWebsite))

                .ForMember(dest => dest.ExpectedDuration,
                    opt => opt.MapFrom(src => src.ExpectedDuration))

                .ForMember(dest => dest.FullDescription,
                    opt => opt.MapFrom(src => src.FullDescription))

                .ForMember(dest => dest.FutureProspects,
                    opt => opt.MapFrom(src => src.FutureProspects))

                .ForMember(dest => dest.OtherInformation,
                    opt => opt.MapFrom(src => src.ImportantOtherInfo))

                .ForMember(dest => dest.InterviewFromDate,
                    opt => opt.MapFrom(src => src.InterviewFromDate))

                .ForMember(dest => dest.IsNasProvider,
                    opt => opt.MapFrom(src => src.IsNasProviderSpecified && src.IsNasProvider))

                .ForMember(dest => dest.IsRecruitmentAgencyAnonymous,
                    opt => opt.MapFrom(src => src.IsRecruitmentAgncyAnonymousSpecified && src.IsRecruitmentAgncyAnonymous))

                 .ForMember(d => d.RecruitmentAgency,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.VacancyManager)
                        ? src.VacancyManagerTradingName
                        : default(string)))

                .ForMember(dest => dest.IsSmallEmployerWageIncentive,
                    opt => opt.MapFrom(src => src.IsSmallEmployerWageIncentiveSpecified && src.IsSmallEmployerWageIncentive))

                // TODO: there is also a Gateway field called LearningProviderSectorPassRate. We are mapping from ApprFrameworkSuccessRate deliberately here.
                .ForMember(d => d.ProviderSectorPassRate,
                    opt => opt.MapFrom(src => src.ApprFrameworkSuccessRateSpecified
                        ? src.ApprFrameworkSuccessRate
                        : default(int?)))

                .ForMember(dest => dest.NumberOfPositions,
                    opt => opt.MapFrom(src => src.NumberOfPositions))

                .ForMember(dest => dest.PersonalQualities,
                    opt => opt.MapFrom(src => src.PersonalQualities))

                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.PossibleStartDate))

                .ForMember(dest => dest.QualificationRequired,
                    opt => opt.MapFrom(src => src.QualificationRequired))

                .ForMember(dest => dest.RealityCheck,
                    opt => opt.MapFrom(src => src.RealityCheck))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.SkillsRequired,
                    opt => opt.MapFrom(src => src.SkillsRequired))

                .ForMember(dest => dest.SupplementaryQuestion1,
                    opt => opt.MapFrom(src => src.SupplementaryQuestion1))

                .ForMember(dest => dest.SupplementaryQuestion2,
                    opt => opt.MapFrom(src => src.SupplementaryQuestion2))

                .ForMember(dest => dest.TradingName,
                    opt => opt.MapFrom(src => src.TradingName))

                .ForMember(dest => dest.ProviderDescription,
                    opt => opt.MapFrom(src => src.TrainingProviderDesc))

                // TODO: AG: US483: check mapping, 'to be provided' versus 'required' does not sound right.
                .ForMember(dest => dest.TrainingToBeProvided,
                    opt => opt.MapFrom(src => src.TrainingRequired))

                .ForMember(dest => dest.VacancyAddress,
                    opt => opt.ResolveUsing<LegacyVacancyDetailAddressDetailsResolver>()
                        .FromMember(src => src.VacancyAddress))

                .ForMember(dest => dest.VacancyLocationType,
                   opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))

                //TODO: Remove once NAS gatway service updated to return correct vacancy address with multi-location vacancies
                .ForMember(dest => dest.IsMultiLocation,
                   opt => opt.ResolveUsing<MultiLocationResolver>().FromMember(src => src.VacancyLocationType))

                .ForMember(dest => dest.VacancyManager,
                    opt => opt.MapFrom(src => src.VacancyManager))

                .ForMember(dest => dest.VacancyOwner,
                    opt => opt.MapFrom(src => src.VacancyOwner))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle))

                .ForMember(dest => dest.ApprenticeshipLevel,
                    opt => opt.ResolveUsing<DetailApprenticeshipLevelResolver>().FromMember(src => src))

                .ForMember(dest => dest.VacancyUrl,
                opt => opt.MapFrom(src => src.ApplyViaEmployerWebsite && string.IsNullOrEmpty(src.VacancyUrl) ? src.EmployerRecruitmentWebsite : src.VacancyUrl))

                .ForMember(dest => dest.WageType,
                    opt => opt.ResolveUsing<WageTypeResolver>().FromMember(src => src.WageType))

                .ForMember(dest => dest.Wage,
                    opt => opt.MapFrom(src => src.WeeklyWage))

                .ForMember(dest => dest.WageDescription,
                    opt => opt.MapFrom(src => src.WageText))

                .ForMember(dest => dest.WorkingWeek,
                    opt => opt.MapFrom(src => src.WorkingWeek))

                .ForMember(dest => dest.Created,
                    opt => opt.Ignore())

                .ForMember(dest => dest.LocalAuthority,
                    opt => opt.Ignore())
            ;
        }
    }
}