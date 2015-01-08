﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using Apprenticeships;
    using Common.Mappers;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;

    public class LegacyVacancySummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            CreateApprenticeshipSummaryMap();
            CreateTraineeshipSummaryMap();
        }

        private void CreateApprenticeshipSummaryMap()
        {
            Mapper.CreateMap<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                //TODO: Change needed on gateway for future requirements
                //.ForMember(dest => dest.VacancyReference,
                //    opt => opt.MapFrom(src => src.VacancyReference))

                //.ForMember(dest => dest.StartDate,
                //    opt => opt.MapFrom(src => src.StartDate))

                .ForMember(dest => dest.VacancyReference,
                    opt => opt.UseValue(null))

                .ForMember(dest => dest.StartDate,
                    opt => opt.UseValue(DateTime.MinValue))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.Location,
                    opt => opt.ResolveUsing<LegacyVacancySummaryLocationResolver>()
                        .FromMember(src => src.Address))

                .ForMember(dest => dest.ApprenticeshipLevel,
                    opt => opt.ResolveUsing<SummaryApprenticeshipLevelResolver>().FromMember(src => src))

                .ForMember(dest => dest.VacancyLocationType,
                    opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle));
        }

        private void CreateTraineeshipSummaryMap()
        {
            Mapper.CreateMap<GatewayServiceProxy.VacancySummary, TraineeshipSummary>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                //TODO: Change needed on gateway for future requirements
                //.ForMember(dest => dest.VacancyReference,
                //    opt => opt.MapFrom(src => src.VacancyReference))

                //.ForMember(dest => dest.StartDate,
                //    opt => opt.MapFrom(src => src.StartDate))

                .ForMember(dest => dest.VacancyReference,
                    opt => opt.UseValue(null))

                .ForMember(dest => dest.StartDate,
                    opt => opt.UseValue(DateTime.MinValue))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.Location,
                    opt => opt.ResolveUsing<LegacyVacancySummaryLocationResolver>()
                        .FromMember(src => src.Address))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle));
        }
    }
}
