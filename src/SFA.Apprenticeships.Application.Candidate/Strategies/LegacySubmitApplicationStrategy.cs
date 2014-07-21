namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public class LegacySubmitApplicationStrategy : ISubmitApplicationStrategy
    {
        // TODO: NOTIMPL: private readonly IApplicationSubmissionQueue _applicationSubmissionQueue;

        public void SubmitApplication(ApplicationDetail application)
        {
            // TODO: NOTIMPL: queue application for submission to legacy
            // TODO: NOTIMPL: if successful then update application status
            // TODO: NOTIMPL: then send email acknowledgement to candidate via ICommunicationService.SendMessageToCandidate()

            throw new NotImplementedException();
        }
    }
}
