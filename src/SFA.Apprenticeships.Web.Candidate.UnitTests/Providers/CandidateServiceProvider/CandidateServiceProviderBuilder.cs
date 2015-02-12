namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System.Web;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Users;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Common.Providers;
    using Common.Services;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Moq;

    public class CandidateServiceProviderBuilder
    {
        protected Mock<ICandidateService> CandidateService;
        protected Mock<IUserAccountService> UserAccountService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<IAuthenticationTicketService> AuthenticationTicketService;
        protected Mock<HttpContextBase> HttpContext;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<ILogService> Logger;
        protected CandidateServiceProvider CandidateServiceProvider;

        public CandidateServiceProviderBuilder()
        {
            CandidateService = new Mock<ICandidateService>();
            UserAccountService = new Mock<IUserAccountService>();
            UserDataProvider = new Mock<IUserDataProvider>();
            AuthenticationTicketService = new Mock<IAuthenticationTicketService>();
            HttpContext = new Mock<HttpContextBase>();
            HttpContext.Setup(h => h.Response).Returns(new Mock<HttpResponseBase>().Object);
            ConfigurationManager = new Mock<IConfigurationManager>();
            Logger = new Mock<ILogService>();
        }

        public CandidateServiceProvider Build()
        {
            CandidateServiceProvider = new CandidateServiceProvider(CandidateService.Object, UserAccountService.Object, UserDataProvider.Object, AuthenticationTicketService.Object, new ApprenticeshipCandidateWebMappers(), HttpContext.Object, ConfigurationManager.Object, Logger.Object);
            return CandidateServiceProvider;
        }

        public CandidateServiceProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            CandidateService = candidateService;
            return this;
        }

        public CandidateServiceProviderBuilder With(Mock<IUserAccountService> userAccountService)
        {
            UserAccountService = userAccountService;
            return this;
        }
    }
}