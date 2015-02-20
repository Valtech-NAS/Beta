namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System;
    using System.Web.Mvc;
    using Controllers;

    public class UserJourneyContextAttribute : ActionFilterAttribute
    {
        private const string UserJourneyKey = "UserJourney";
        private const string ApprenticeshipsMainCaption = "Apprenticeships";
        private const string TraineeshipsMainCaption = "Traineeships";

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
                if (candidateController.UserData.Get(UserJourneyKey) != null && UserJourney == UserJourney.None)
                {
                    candidateController.ViewBag.UserJourney =
                        (UserJourney) Enum.Parse(typeof (UserJourney), candidateController.UserData.Get(UserJourneyKey));
                }
                else if (candidateController.UserData.Get(UserJourneyKey) == null && UserJourney == UserJourney.None)
                {
                    candidateController.UserData.Push(UserJourneyKey, UserJourney.Apprenticeship.ToString());
                    candidateController.ViewBag.UserJourney =
                        (UserJourney) Enum.Parse(typeof (UserJourney), candidateController.UserData.Get(UserJourneyKey));
                }
                else
                {
                    candidateController.UserData.Push(UserJourneyKey, UserJourney.ToString());
                    candidateController.ViewBag.UserJourney = UserJourney;
                }

                candidateController.ViewBag.UserJourneyMainCaption =
                        GetMainCaption(candidateController.ViewBag.UserJourney);
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