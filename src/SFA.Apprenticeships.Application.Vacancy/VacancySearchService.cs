namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Location;
    using Interfaces.Vacancy;
    using Interfaces.Location;
    using Domain.Entities.Vacancy;

    //todo: implement vacancy search service
    public class VacancySearchService : IVacancySearchService
    {
        private readonly ILocationLookupProvider _locationLookupProvider;
        private readonly IVacancySearchProvider _vacancySearchProvider;
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public VacancySearchService(ILocationLookupProvider locationLookupProvider, IVacancySearchProvider vacancySearchProvider, IPostcodeLookupProvider postcodeLookupProvider)
        {
            Condition.Requires(locationLookupProvider).IsNotNull();

            _locationLookupProvider = locationLookupProvider;
            _vacancySearchProvider = vacancySearchProvider;
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public IEnumerable<VacancySummary> Search(string postcodeOrLocationName, int searchRadius)
        {
            Condition.Requires(postcodeOrLocationName, "postcodeOrLocationName").IsNotNullOrEmpty();
            Condition.Requires(searchRadius, "searchRadius").IsGreaterOrEqual(0);

            // if a postcode then retrieve Location from postcode, otherwise assume a place name
            var location = LocationHelper.IsPostcode(postcodeOrLocationName)
                ? _postcodeLookupProvider.GetLocation(postcodeOrLocationName)
                : _locationLookupProvider.GetLocation(postcodeOrLocationName);

            Condition.Requires(location).IsNotNull(string.Format("Invalid postcode or place name '{0}'", postcodeOrLocationName));

            var vacancies = _vacancySearchProvider.FindVacancies(location, searchRadius);

            return vacancies;
        }
    }
}
