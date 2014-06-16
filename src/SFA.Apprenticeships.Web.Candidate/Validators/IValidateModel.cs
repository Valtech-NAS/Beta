
namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System.Web.Mvc;

    public interface IValidateModel<in T>
    {
        bool Validate(T model, ModelStateDictionary modelState);
    }
}
