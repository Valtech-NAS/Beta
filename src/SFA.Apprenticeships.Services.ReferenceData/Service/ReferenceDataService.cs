using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SFA.Apprenticeships.Common.Configuration;
using SFA.Apprenticeships.Common.Entities.ReferenceData;
using SFA.Apprenticeships.Services.Common.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Models;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;

namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    public class ReferenceDataService : IReferenceDataService
    {
        public const string ReferenceDataPublicKey = "ReferenceDataService.Password";
        public const string ReferenceDataSystemIdKey = "ReferenceDataService.Username";

        private readonly IWcfService<IReferenceData> _service;
        private readonly Guid _systemId;
        private readonly string _publicKey;

        public ReferenceDataService(IConfigurationManager configManager, IWcfService<IReferenceData> service)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException("configManager");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _systemId = new Guid(configManager.GetAppSetting(ReferenceDataSystemIdKey));
            _publicKey = configManager.GetAppSetting(ReferenceDataPublicKey);
            _service = service;
        }

        public IEnumerable<ILegacyReferenceData> GetReferenceData(LegacyReferenceDataType type)
        {
            switch (type)
            {
                case LegacyReferenceDataType.County:
                    return GetCounties();
                case LegacyReferenceDataType.ErrorCode:
                    return GetErrorCodes();
                case LegacyReferenceDataType.Occupations:
                    return GetApprenticeshipOccupations();
                case LegacyReferenceDataType.Framework:
                    return GetApprenticeshipFrameworks();
                case LegacyReferenceDataType.LocalAuthority:
                    return GetLocalAuthorities();
                case LegacyReferenceDataType.Region:
                    return GetRegions();
                default:
                    throw new NotImplementedException(string.Format("Legacy reference type '{0}' not implemented.", type));
            }
        }

        public IEnumerable<ILegacyReferenceData> GetApprenticeshipOccupations()
        {
            var data = GetApprenticeshipFrameworks();

            if (data != null)
            {
                return (data as IEnumerable<Framework>).Select(
                    item => new Occupation
                    {
                        Id = item.Occupation.Id,
                        ShortName = item.Occupation.ShortName,
                        Description = item.Occupation.Description
                    });
            }

            return default(IList<Occupation>);
        }

        public IEnumerable<ILegacyReferenceData> GetApprenticeshipFrameworks()
        {
            var msgId = Guid.NewGuid();
            var request = new GetApprenticeshipFrameworksRequest
            {
                ExternalSystemId = _systemId,
                PublicKey = _publicKey,
                MessageId = msgId,
            };

            var rs = default(GetApprenticeshipFrameworksResponse);
            _service.Use(client => { rs = client.GetApprenticeshipFrameworks(request); });

            if (rs != null)
            {
                return rs.ApprenticeshipFrameworks.Select(
                    item => new Framework
                    {

                        Id = item.ApprenticeshipFrameworkCodeName,
                        ShortName = item.ApprenticeshipFrameworkShortName,
                        Description = item.ApprenticeshipFrameworkFullName,
                        Occupation = new Occupation
                        {
                            Id = item.ApprenticeshipOccupationCodeName,
                            ShortName = item.ApprenticeshipOccupationShortName,
                            Description = item.ApprenticeshipOccupationFullName
                        }
                    });
            }

            return default(IEnumerable<Framework>);
        }

        public IEnumerable<ILegacyReferenceData> GetCounties()
        {
            var msgId = Guid.NewGuid();
            var request = new GetCountiesRequest
            {
                ExternalSystemId = _systemId,
                PublicKey = _publicKey,
                MessageId = msgId,
            };

            var rs = default(GetCountiesResponse);
            _service.Use(client => { rs = client.GetCounties(request); });

            if (rs != null)
            {
                return rs.Counties.Select(
                    item => new County
                    {
                        Id = item.CodeName,
                        Description = item.FullName
                    });
            }

            return default(IEnumerable<County>);
        }

        public IEnumerable<ILegacyReferenceData> GetErrorCodes()
        {
            var msgId = Guid.NewGuid();
            var request = new GetErrorCodesRequest
            {
                ExternalSystemId = _systemId,
                PublicKey = _publicKey,
                MessageId = msgId,
            };

            var rs = default(GetErrorCodesResponse);
            _service.Use(client => { rs = client.GetErrorCodes(request); });

            if (rs != null)
            {
                return rs.ErrorCodes.Select(
                    item => new ErrorCode
                    {
                        Id = item.InterfaceErrorCode.ToString(CultureInfo.InvariantCulture),
                        Description = item.InterfaceErrorDescription
                    });
            }

            return default(IEnumerable<ErrorCode>);
        }

        public IEnumerable<ILegacyReferenceData> GetLocalAuthorities()
        {
            var msgId = Guid.NewGuid();
            var request = new GetLocalAuthoritiesRequest
            {
                ExternalSystemId = _systemId,
                PublicKey = _publicKey,
                MessageId = msgId,
            };

            var rs = default(GetLocalAuthoritiesResponse);
            _service.Use(client => { rs = client.GetLocalAuthorities(request); });

            if (rs != null)
            {
                return rs.LocalAuthorities.Select(
                    item => new LocalAuthority
                    {
                        County = item.County,
                        Description = item.FullName,
                        ShortName = item.ShortName,
                        Id = item.ShortName,
                    });
            }

            return default(IList<LocalAuthority>);
        }

        public IEnumerable<ILegacyReferenceData> GetRegions()
        {
            var msgId = Guid.NewGuid();
            var request = new GetRegionRequest
            {
                ExternalSystemId = _systemId,
                PublicKey = _publicKey,
                MessageId = msgId,
            };

            var rs = default(GetRegionResponse);
            _service.Use(client => { rs = client.GetRegion(request); });

            if (rs != null)
            {
                return rs.Regions.Select(
                    item => new Region
                    {
                        Id = item.CodeName,
                        Description = item.FullName,
                    });
            }

            return default(IEnumerable<Region>);
        }
    }
}
