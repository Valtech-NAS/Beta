namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Providers;
    using Providers;

    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        private IUserDataProvider _userData;

        protected CandidateControllerBase(ISessionStateProvider session) : base(session) { }

        protected IUserDataProvider UserData
        {
            get { return _userData ?? (_userData = new CookieUserDataProvider(HttpContext)); }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserContext = null;

            if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var context = UserData.GetUserContext();

                if (context != null)
                {
                    var candidateContext = new CandidateUserContext
                    {
                        CandidateId = new Guid(User.Identity.Name),
                        FullName = context.FullName,
                        UserName = context.UserName
                    };

                    UserContext = candidateContext;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
