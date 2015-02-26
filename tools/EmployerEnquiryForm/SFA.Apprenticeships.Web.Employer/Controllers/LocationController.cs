namespace SFA.Apprenticeships.Web.Employer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Mediators.Interfaces;
    using Providers.Interfaces;
    using ViewModels;

    public class LocationController : Controller
    {
        private readonly ILocationMediator _locationMediator;

        public LocationController(ILocationMediator locationMediator)
        {
            _locationMediator = locationMediator;
        }


        [HttpGet]
        public async Task<ActionResult> Addresses(string postcode)
        {
            return await Task.Run<ActionResult>(() =>
            {
                if (Request.IsAjaxRequest())
                {
                    var addresses = _locationMediator.FindAddress(postcode).ViewModel;
                    return Json(addresses, JsonRequestBehavior.AllowGet);
                }

                throw new NotSupportedException("Non-JS not yet implemented!");
            });
        }
    }
}
