namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;

    public interface ISendMobileVerificationCodeStrategy
    {
        void SendMobileVerificationCode(Candidate candidate);
    }
}