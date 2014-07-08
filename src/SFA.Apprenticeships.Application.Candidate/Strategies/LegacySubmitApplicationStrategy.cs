namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        //todo: private readonly IApplicationSubmissionQueue _applicationSubmissionQueue;

        public void SubmitApplication(ApplicationDetail application)
        {
            //todo: create/update application status
            //todo: queue application for submission to legacy
            //todo: send email acknowledgement to candidate via ICommunicationService.SendMessageToCandidate()

            throw new NotImplementedException();
        }
    }
}
