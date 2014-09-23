namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Attributes;
    using Constants;
    using Constants.ViewModels;
    using Domain.Interfaces.Configuration;
    using Providers;
    using ViewModels.VacancySearch;

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
        public ActionResult Location(string term)
        {
            LocationsViewModel result = _searchProvider.FindLocation(term);

            if (Request.IsAjaxRequest())
            {
                return Json(result.Locations.Take(_locationResultLimit), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [OutputCache(CacheProfile = CacheProfiles.Data, VaryByParam = "postcode")]
        public ActionResult Addresses(string postcode)
        {
            AddressSearchResult addresses = _searchProvider.FindAddresses(postcode);
            if (Request.IsAjaxRequest())
            {
                return Json(addresses, JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }
    }
}