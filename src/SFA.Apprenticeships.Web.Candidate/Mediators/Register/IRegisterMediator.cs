namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using ViewModels.Register;

    public interface IRegisterMediator
    {
        MediatorResponse<RegisterViewModel> Register(RegisterViewModel registerViewModel);

        MediatorResponse<ActivationViewModel> Activate(Guid candidateId, ActivationViewModel activationViewModel);

        //todo: move to login mediator
        MediatorResponse<ForgottenPasswordViewModel> ForgottenPassword(ForgottenPasswordViewModel forgottenPasswordViewModel);

        //todo: move to login mediator
        MediatorResponse<PasswordResetViewModel> ResetPassword(PasswordResetViewModel resetViewModel);
    }
}
