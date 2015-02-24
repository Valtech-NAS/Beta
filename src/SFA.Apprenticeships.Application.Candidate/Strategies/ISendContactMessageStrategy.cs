namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Communication;

    public interface ISendContactMessageStrategy
    {
        void SendMessage(ContactMessage message);
    }
}