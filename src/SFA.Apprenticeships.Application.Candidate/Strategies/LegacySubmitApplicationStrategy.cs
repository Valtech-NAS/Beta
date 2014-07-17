namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        // TODO: NOTIMPL: private readonly IApplicationSubmissionQueue _applicationSubmissionQueue;

        public void SubmitApplication(ApplicationDetail application)
        {
            // TODO: NOTIMPL: create/update application status
            // TODO: NOTIMPL: queue application for submission to legacy
            // TODO: NOTIMPL: send email acknowledgement to candidate via ICommunicationService.SendMessageToCandidate()

            throw new NotImplementedException();
        }
    }
}
