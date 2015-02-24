namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using ViewModels.Home;

    public interface IHomeProvider
    {
        bool SendContactMessage(Guid? candidateId, ContactMessageViewModel viewModel);
    }
}