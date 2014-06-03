using CuttingEdge.Conditions;
using SFA.Apprenticeships.Web.Common.Providers;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;

    public class AdminController : Controller
    {
        private readonly IWebReferenceDataProvider _provider;

        public AdminController(IWebReferenceDataProvider provider)
        {
            Condition.Requires(provider, "provider").IsNotNull();

            _provider = provider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Frameworks()
        {
            var data = _provider.Get(LegacyReferenceDataType.Framework);
            ViewBag.LegacyType = LegacyReferenceDataType.Framework;
            return View("referencedata", data);
        }

        public ActionResult Occupations()
        {
            var data = _provider.Get(LegacyReferenceDataType.Occupations);
            ViewBag.LegacyType = LegacyReferenceDataType.Occupations;
            return View("referencedata", data);
        }

        public ActionResult ErrorCodes()
        {
            var data = _provider.Get(LegacyReferenceDataType.ErrorCode);
            ViewBag.LegacyType = LegacyReferenceDataType.ErrorCode;
            return View("referencedata", data);
        }

        public ActionResult Counties()
        {
            var data = _provider.Get(LegacyReferenceDataType.County);
            ViewBag.LegacyType = LegacyReferenceDataType.County;
            return View("referencedata", data);
        }

        public ActionResult LocalAuthorities()
        {
            var data = _provider.Get(LegacyReferenceDataType.LocalAuthority);
            ViewBag.LegacyType = LegacyReferenceDataType.LocalAuthority;
            return View("referencedata", data);
        }

        public ActionResult Regions()
        {
            var data = _provider.Get(LegacyReferenceDataType.Region);
            ViewBag.LegacyType = LegacyReferenceDataType.Region;
            return View("referencedata", data);
        }
	}
}