using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SFA.Apprenticeships.Services.Common.Configuration;
using SFA.Apprenticeships.Services.Common.Wcf;
using SFA.Apprenticeships.Services.Models.ReferenceDataModels;
using SFA.Apprenticeships.Services.ReferenceData.Abstract;
using SFA.Apprenticeships.Services.ReferenceData.Proxy;

namespace SFA.Apprenticeships.Services.ReferenceData.Service
{
    public class ReferenceDataService : IReferenceDataService
    {
        private const string ReferenceDataPassword = "ReferenceDataService.Password";
        private const string ReferenceDataUsername = "ReferenceDataService.Username";

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

            _systemId = new Guid(configManager.GetAppSetting(ReferenceDataUsername));
            _publicKey = configManager.GetAppSetting(ReferenceDataPassword);
            _service = service;
        }

        public IList<Framework> GetApprenticeshipFrameworks()
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

                        CodeName = item.ApprenticeshipFrameworkCodeName,
                        ShortName = item.ApprenticeshipFrameworkShortName,
                        FullName = item.ApprenticeshipFrameworkFullName,
                        Occupation = new Occupation
                        {
                            CodeName = item.ApprenticeshipOccupationCodeName,
                            ShortName = item.ApprenticeshipOccupationShortName,
                            FullName = item.ApprenticeshipOccupationFullName
                        }
                    })
                    .ToList();
            }

            return default(IList<Framework>);
        }

        public IList<County> GetCounties()
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
                        CodeName = item.CodeName,
                        FullName = item.FullName
                    })
                    .ToList();
            }

            return default(IList<County>);
        }

        public IList<ErrorCode> GetErrorCodes()
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
                        CodeName = item.InterfaceErrorCode.ToString(CultureInfo.InvariantCulture),
                        FullName = item.InterfaceErrorDescription
                    })
                    .ToList();
            }

            return default(IList<ErrorCode>);
        }

        public IList<LocalAuthority> GetLocalAuthorities()
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
                        FullName = item.FullName,
                        ShortName = item.ShortName,
                        CodeName = item.ShortName,
                    })
                    .ToList();
            }

            return default(IList<LocalAuthority>);
        }

        public IList<Region> GetRegions()
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
                        CodeName = item.CodeName,
                        FullName = item.FullName,
                    })
                    .ToList();
            }

            return default(IList<Region>);
        }
    }
}
