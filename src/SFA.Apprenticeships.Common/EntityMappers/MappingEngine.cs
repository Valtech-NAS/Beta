using System;
using AutoMapper;
using SFA.Apprenticeships.Common.Interfaces.Mapper;

namespace SFA.Apprenticeships.Common.EntityMappers
{
    public abstract class MappingEngine : IMapper
    {
        private readonly IMappingEngine _mappingEngine;

        protected MappingEngine(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var map = _mappingEngine.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);

            if (map != null)
            {
                return _mappingEngine.Map(source, sourceType, destinationType);
            }

            throw new InvalidOperationException("No mapping configuration registered for mapping " + sourceType.FullName +
                                                " to " + destinationType.FullName);
        }

        public TDestination Map<TSource, TDestination>(TSource sourceObject)
        {
            return (TDestination) Map(sourceObject, typeof (TSource), typeof (TDestination));
        }
    }

    //public class AutomapperWebConfiguration
    //{
    //    private static IMappingEngine _webMapper;
    //    private static readonly object _syncRoot = new Object();

    //    private AutomapperWebConfiguration()
    //    {
    //    }

    //    // multi-threaded mapper config.
    //    public static IMappingEngine WebMapper
    //    {
    //        get
    //        {
    //            if (_webMapper == null)
    //            {
    //                lock (_syncRoot)
    //                {
    //                    if (_webMapper == null)
    //                    {
    //                        var configuration = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

    //                        _webMapper = new MappingEngine(configuration);
    //                    }
    //                }
    //            }

    //            return _webMapper;
    //        }
    //    }
    //}

    //static class SearchProfile 
    //{
    //    public static void Configure(ConfigurationStore store)
    //    {
    //        store.CreateMap<ExtendedSearchViewModel, QueryParameters>()
    //            .ForMember(d => d.Postcode, x => x.MapFrom(s => s.SearchByPostcode ? s.Postcode : string.Empty))
    //            .ForMember(d => d.VacancyType,
    //                x =>
    //                    x.MapFrom(
    //                        s =>
    //                            s.SearchByVacancyType
    //                                ? s.VacancyType != "Any" ? s.VacancyType : string.Empty
    //                                : string.Empty))
    //            .ForMember(d => d.Title, x => x.MapFrom(s => s.SearchByVacancy ? s.Title : string.Empty))
    //            .ForMember(d => d.Provider, x => x.MapFrom(s => s.SearchByTrainingProvider ? s.Provider : string.Empty))
    //            .ForMember(d => d.Employer, x => x.MapFrom(s => s.SearchByEmployer ? s.Employer : string.Empty))
    //            .ForMember(d => d.SortBy, x => x.MapFrom(s => s.SortBy))
    //            .ForMember(d => d.SortDirection, x => x.MapFrom(s => s.SortDirection))
    //            .ForMember(d => d.PostDate, x => x.ResolveUsing(s =>
    //            {
    //                var postDate = new Range<DateTime> {HasValue = s.SearchByVacancyPostDate};
    //                DateTime date;
    //                postDate.From = DateTime.TryParse(s.PostDate, out date) ? date : DateTime.Today.AddMonths(-1);
    //                return postDate;
    //            }))
    //            .ForMember(d => d.Hours, x => x.ResolveUsing(s =>
    //            {
    //                var hours = new Range<double> {HasValue = s.SearchByHoursPerWeek};
    //                double value;
    //                hours.From = double.TryParse(s.HoursFrom, out value) ? value : default(double);
    //                hours.To = double.TryParse(s.HoursTo, out value) ? value : default(double);
    //                return hours;
    //            }))
    //            .ForMember(d => d.Wage, x => x.ResolveUsing(s =>
    //            {
    //                var wage = new Range<double> {HasValue = s.SearchByWeeklyWage};
    //                double value;
    //                wage.From = double.TryParse(s.WageFrom, out value) ? value : default(double);
    //                wage.To = double.TryParse(s.WageTo, out value) ? value : default(double);
    //                return wage;
    //            }))
    //            .ForMember(d => d.Location, x => x.ResolveUsing(s =>
    //            {
    //                var location = new GeoLocation {HasValue = s.SearchByDistance};
    //                if (s.SearchByDistance)
    //                {
    //                    double value;
    //                    if (double.TryParse(s.Distance, out value))
    //                    {
    //                        location.Distance = value;
    //                    }

    //                    if (!string.IsNullOrEmpty(s.Postcode))
    //                    {
    //                        var postcodeService = new PostcodeService();
    //                        var postcodeInfo = postcodeService.GetPostcodeInfo(s.Postcode);
    //                        if (postcodeInfo != null)
    //                        {
    //                            location.lat = postcodeInfo.Latitude;
    //                            location.lon = postcodeInfo.Longitude;
    //                        }
    //                    }
    //                }
    //                return location;
    //            }))
    //            ;
    //    }
}