namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using Constants;
    using Providers;
    using ViewModels.VacancySearch;
    using Common.Attributes;

    public class LocationController : Controller
    {
        private readonly int _locationResultLimit;
        private readonly ISearchProvider _searchProvider;

        public LocationController(IConfigurationManager configManager, ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
            _locationResultLimit = configManager.GetAppSetting<int>("LocationResultLimit");
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.Data, VaryByParam = "term")]
        public async Task<ActionResult> Location(string term)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var result = string.IsNullOrWhiteSpace(term)
                    ? new LocationsViewModel(new List<LocationViewModel>())
                    : _searchProvider.FindLocation(term);

                if (Request.IsAjaxRequest())
                {
                    return Json(result.Locations.Take(_locationResultLimit), JsonRequestBehavior.AllowGet);
                }

                throw new NotImplementedException("Non-js not yet implemented!");
            });
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.Data, VaryByParam = "postcode")]
        public async Task<ActionResult> Addresses(string postcode)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var addresses = _searchProvider.FindAddresses(postcode);
                if (Request.IsAjaxRequest())
                {
                    return Json(addresses, JsonRequestBehavior.AllowGet);
                }

                throw new NotImplementedException("Non-js not yet implemented!");
            });
        }
    }
}
