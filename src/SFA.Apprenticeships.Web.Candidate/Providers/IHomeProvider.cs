namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Home;

    //todo: refactor to candidate provider
    public interface IHomeProvider
    {
        bool SendContactMessage(Guid? candidateId, ContactMessageViewModel viewModel);
    }
}
