namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Controllers;
    using Common.Providers;
    using Providers;

    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        protected CandidateControllerBase(ISessionStateProvider session, IUserServiceProvider userServiceProvider)
            : base(session)
        {
            UserServiceProvider = userServiceProvider;            
        }

        protected IUserServiceProvider UserServiceProvider { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserContext = null;

            if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var context = UserServiceProvider.GetUserContext(HttpContext);

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
