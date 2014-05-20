namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Service
{
    using System;
    using SFA.Apprenticeships.Common.Entities.Vacancy;
    using SFA.Apprenticeships.Common.Interfaces.Services;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

    public class VacancySummaryService : IVacancySummaryService
    {
        public const string ReferenceDataPublicKey = "ReferenceDataService.Password";
        public const string ReferenceDataSystemIdKey = "ReferenceDataService.Username";

        private readonly IWcfService<IVacancySummary> _service;
        private readonly Guid _systemId;
        private readonly string _publicKey;

        public VacancySummaryService(IWcfService<IVacancySummary> service)
        {
            _service = service;
        }

        public VacancySummary GetVacancySummary(int page = 1)
        {
            
        }
    }
}
