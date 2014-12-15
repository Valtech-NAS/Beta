namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Applications;

    public interface ILegacyApplicationProvider
    {
        int CreateApplication(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail);
    }
}
