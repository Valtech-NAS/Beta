namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;

    public abstract class TestsBase
    {
        protected static IApprenticeshipApplicationMediator GetMediator(IApprenticeshipApplicationProvider apprenticeshipApplicationProvider, IConfigurationManager configurationManager, IUserDataProvider userDataProvider)
        {
            var mediator = new ApprenticeshipApplicationMediator(apprenticeshipApplicationProvider, new ApprenticeshipApplicationViewModelServerValidator(), new ApprenticeshipApplicationViewModelSaveValidator(), configurationManager, userDataProvider);
            return mediator;
        }
    }
}