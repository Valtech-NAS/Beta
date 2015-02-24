namespace SFA.Apprenticeships.Web.Candidate.Mediators.Home
{
    using System;
    using ViewModels.Home;

    public interface IHomeMediator
    {
        MediatorResponse<ContactMessageViewModel> SendContactMessage(Guid? candidateId, ContactMessageViewModel registerViewModel);

        MediatorResponse<ContactMessageViewModel> GetContactMessageViewModel(Guid? candidateId);
    }
}