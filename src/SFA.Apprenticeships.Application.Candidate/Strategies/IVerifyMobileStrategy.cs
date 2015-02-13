 

namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IVerifyMobileStrategy
    {
        void VerifyMobile(Guid candidateId, string verificationCode);
    }

   
}
