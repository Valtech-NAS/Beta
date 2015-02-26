namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;

    public interface ISubmitContactMessageStrategy
    {
        void SubmitMessage(ContactMessage message);
    }
}
