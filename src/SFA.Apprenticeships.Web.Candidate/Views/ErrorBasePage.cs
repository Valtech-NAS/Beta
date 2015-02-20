namespace SFA.Apprenticeships.Web.Candidate.Views
{
    using System.Web.UI;

    public class ErrorBasePage : Page
    {
        protected void SetTitle(string errorCode)
        {
            Page.Title = string.Format("{0}s - {1}", GetUserJourney(), errorCode);
        }

        protected string GetUserJourney()
        {
            var userJourney = "Apprenticeships";

            if (Request.Cookies["User.Data"] != null)
            {
                if (Request.Cookies["User.Data"]["UserJourney"] != null)
                {
                    userJourney = Request.Cookies["User.Data"]["UserJourney"];
                }
            }

            return userJourney;
        }
    }
}