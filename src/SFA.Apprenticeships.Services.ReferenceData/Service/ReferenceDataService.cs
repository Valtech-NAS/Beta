namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataService _service;

        public ReferenceDataService(IReferenceDataService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            _service = service;
        }

        public IEnumerable<Occupation> GetApprenticeshipOccupations()
        {
            return _service.GetApprenticeshipOccupations();
        }

        public IEnumerable<Framework> GetApprenticeshipFrameworks()
        {
            return _service.GetApprenticeshipFrameworks();
        }

        public IEnumerable<County> GetCounties()
        {
            return _service.GetCounties();
        }

        public IEnumerable<ErrorCode> GetErrorCodes()
        {
            return _service.GetErrorCodes();
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            return _service.GetLocalAuthorities();
        }

        public IEnumerable<Region> GetRegions()
        {
            return _service.GetRegions();
        }
    }
}
