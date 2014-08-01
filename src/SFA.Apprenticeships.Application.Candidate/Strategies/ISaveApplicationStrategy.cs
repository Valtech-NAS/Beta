namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Applications;

    public interface ISaveApplicationStrategy
    {
        ApplicationDetail SaveApplication(ApplicationDetail application);
    }
}