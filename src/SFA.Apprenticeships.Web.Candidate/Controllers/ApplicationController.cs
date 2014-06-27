namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Controllers;
    using Infrastructure.Azure.Session;
    using Providers;
    using ViewModels.Applications;
    using ViewModels.Candidate;
    using ViewModels.Locations;

    public class ApplicationController : SfaControllerBase
    {
        private const string ApplyForVacancyId = "ApplyForVacancyId";

        public ApplicationController(ISessionState sessionState) : base(sessionState)
        {
        }

        public ActionResult Index(int id)
        {
            Session.Store(ApplyForVacancyId, id);
            return View(id);
        }

        public ActionResult Apply(int id, int profileId)
        {
            var x = new ApplicationViewModel
            {
                EmployerName = "Emp name",
                VacancySummary = "Vac summary",
                VacancyTitle = "Vac title",
                Candidate = new CandidateViewModel
                {
                    FullName = "Full name",
                    DateOfBirth = DateTime.Today,
                    EmailAddress = "email@asdf.com",
                    PhoneNumber = "495872349857",
                    Address = new AddressViewModel(),
                    AboutYou = new AboutYouViewModel()
                }
            };


            return View(x);
        }
    }
}