namespace SFA.Apprenticeships.Web.Employer.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using Mediators;
    using Mediators.Interfaces;
    using ViewModels;
    using FluentValidation.Mvc;
    using Framework.Attributes;
    using Mediators.EmployerEnquiry;

    public class EmployerEnquiryController : EmployerControllerBase
    {
        private IEmployerEnquiryMediator _employerEnquiryMediator;

        public EmployerEnquiryController(IEmployerEnquiryMediator employerEnquiryMediator)
        {
            _employerEnquiryMediator = employerEnquiryMediator;
        }

        [HttpGet]
        public async Task<ActionResult> SubmitEmployerEnquiry()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var result = _employerEnquiryMediator.SubmitEnquiry();
                return View(result.ViewModel);
            });
        }
        
        [HttpPost]
        [HoneypotCaptcha("UserName")]
        public async Task<ActionResult> SubmitEmployerEnquiry(EmployerEnquiryViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _employerEnquiryMediator.SubmitEnquiry(model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Error:
                        SetPageMessage(EmployerEnquiryPageMessages.ErrorWhileQuerySubmission, UserMessageLevel.Error);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Success:
                        SetPageMessage(EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully);
                        return View("ThankYou");
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        public async Task<ActionResult> ThankYou()
        {
            return await Task.Run<ActionResult>(() => View());
        }
    }
}