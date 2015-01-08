namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    using System;
    using ViewModels.Register;

    public interface IRegisterMediator
    {
        MediatorResponse<RegisterViewModel> Register(RegisterViewModel registerViewModel);

        MediatorResponse<ActivationViewModel> Activate(Guid candidateId, ActivationViewModel activationViewModel);

        MediatorResponse<ForgottenPasswordViewModel> ForgottenPassword(ForgottenPasswordViewModel forgottenPasswordViewModel);

        MediatorResponse<PasswordResetViewModel> ResetPassword(PasswordResetViewModel resetViewModel);
    }
}
