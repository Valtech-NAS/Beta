namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using Domain.Entities.Applications;

    public interface ISaveApprenticeshipApplicationStrategy
    {
        ApprenticeshipApplicationDetail SaveApplication(ApprenticeshipApplicationDetail apprenticeshipApplication);
    }
}