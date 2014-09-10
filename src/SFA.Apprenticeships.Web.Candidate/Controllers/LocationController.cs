namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Common.Attributes;
    using Constants;
    using Domain.Interfaces.Configuration;
    using Providers;

    public class LocationController : Controller
    {
        private readonly ISearchProvider _searchProvider;
        private readonly int _locationResultLimit;

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
            var result = _searchProvider.FindLocation(term);

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
            var addresses = _searchProvider.FindAddresses(postcode);
            if (Request.IsAjaxRequest())
            {
                return Json(addresses, JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }
    }
}