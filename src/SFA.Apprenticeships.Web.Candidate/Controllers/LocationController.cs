namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Attributes;
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
        public ActionResult Location(string term)
        {
            var matches = _searchProvider.FindLocation(term);

            if (Request.IsAjaxRequest())
            {
                return Json(matches.Take(_locationResultLimit), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Addresses(string postcode)
        {
            var matches = _searchProvider.FindAddresses(postcode);

            if (Request.IsAjaxRequest())
            {
                return Json(matches.OrderBy(a => a.Uprn), JsonRequestBehavior.AllowGet);
            }

            throw new NotImplementedException("Non-js not yet implemented!");
        }
    }
}