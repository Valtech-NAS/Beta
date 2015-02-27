namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System;
    using System.Web.Mvc;
    using Controllers;

    public class UserJourneyContextAttribute : ActionFilterAttribute
    {
        private const string UserJourneyKey = "UserJourney";
        private const string ApprenticeshipsMainCaption = "Find an apprenticeship";
        private const string TraineeshipsMainCaption = "Find a traineeship";

        public UserJourney UserJourney { get; set; }

        public UserJourneyContextAttribute(UserJourney userJourney)
        {
            UserJourney = userJourney;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var candidateController = filterContext.Controller as CandidateControllerBase;

            if (candidateController != null )
            {
                var userDataProvider = candidateController.UserData;
                var userJourneyValue = userDataProvider.Get(UserJourneyKey);
                if (userJourneyValue != null && UserJourney == UserJourney.None)
                {
                    candidateController.ViewBag.UserJourney = (UserJourney) Enum.Parse(typeof (UserJourney), userJourneyValue);
                }
                else if (userJourneyValue == null && UserJourney == UserJourney.None)
                {
                    userDataProvider.Push(UserJourneyKey, UserJourney.Apprenticeship.ToString());
                    candidateController.ViewBag.UserJourney = UserJourney.Apprenticeship;
                }
                else
                {
                    userDataProvider.Push(UserJourneyKey, UserJourney.ToString());
                    candidateController.ViewBag.UserJourney = UserJourney;
                }

                candidateController.ViewBag.UserJourneyMainCaption = GetMainCaption(candidateController.ViewBag.UserJourney);
            }

            base.OnActionExecuting(filterContext);
        }

        private static string GetMainCaption(UserJourney userJourney)
        {
            return userJourney == UserJourney.Apprenticeship ? ApprenticeshipsMainCaption : TraineeshipsMainCaption;
        }
    }

    public enum UserJourney
    {
        Apprenticeship,
        Traineeship,
        None
    }
}