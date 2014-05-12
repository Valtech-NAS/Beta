using System;
using System.Collections.Generic;
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

        private readonly IConfigurationManager _configManager;

        public ReferenceDataService(IConfigurationManager configManager)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException("configManager");
            }

            _configManager = configManager;
        }

        public IList<Framework> GetApprenticeshipFrameworks()
        {
            var msgId = Guid.NewGuid();
            var request = new GetApprenticeshipFrameworksRequest
            {
                ExternalSystemId = new Guid(_configManager.GetAppSetting(ReferenceDataUsername)),
                PublicKey = _configManager.GetAppSetting(ReferenceDataPassword),
                MessageId = msgId,
            };

            var rs = default(GetApprenticeshipFrameworksResponse);
            WcfService<IReferenceData>.Use(client => { rs = client.GetApprenticeshipFrameworks(request); });

            if (rs != null)
            {
                return rs.ApprenticeshipFrameworks.Select(
                    item => new Framework
                    {
                        CodeName = item.ApprenticeshipFrameworkCodeName,
                        ShortName = item.ApprenticeshipFrameworkShortName,
                        FullName = item.ApprenticeshipFrameworkFullName
                    })
                    .ToList();
            }

            return default(IList<Framework>);
        }

        public IList<Occupation> GetApprenticeshipOccupations()
        {
            var msgId = Guid.NewGuid();
            var request = new GetApprenticeshipFrameworksRequest
            {
                ExternalSystemId = new Guid(_configManager.GetAppSetting(ReferenceDataUsername)),
                PublicKey = _configManager.GetAppSetting(ReferenceDataPassword),
                MessageId = msgId,
            };

            var rs = default(GetApprenticeshipFrameworksResponse);
            WcfService<IReferenceData>.Use(client => { rs = client.GetApprenticeshipFrameworks(request); });

            if (rs != null)
            {
                return rs.ApprenticeshipFrameworks.Select(
                    item => new Occupation
                    {
                        CodeName = item.ApprenticeshipOccupationCodeName,
                        ShortName = item.ApprenticeshipOccupationShortName,
                        FullName = item.ApprenticeshipOccupationFullName
                    })
                    .ToList();
            }

            return default(IList<Occupation>);
        }
    }
}
