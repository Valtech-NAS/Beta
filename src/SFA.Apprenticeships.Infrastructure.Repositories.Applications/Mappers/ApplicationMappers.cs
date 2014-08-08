﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Applications;
    using Entities;

    public class ApplicationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseApplicationDetailMappers();
            InitialiseApplicationSummaryMappers();
        }

        private void InitialiseApplicationDetailMappers()
        {
            Mapper.CreateMap<ApplicationDetail, MongoApplicationDetail>();
            Mapper.CreateMap<MongoApplicationDetail, ApplicationDetail>();
        }

        private void InitialiseApplicationSummaryMappers()
        {
            Mapper.CreateMap<MongoApplicationDetail, ApplicationSummary>()
                .ForMember(x => x.ApplicationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.LegacyVacancyId, opt => opt.MapFrom(src => src.LegacyApplicationId))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Vacancy.Title))
                .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(x => x.DateUpdated, opt => opt.MapFrom(src => src.DateUpdated))
                .ForMember(x => x.DateApplied, opt => opt.MapFrom(src => src.DateApplied));
        }
    }
}