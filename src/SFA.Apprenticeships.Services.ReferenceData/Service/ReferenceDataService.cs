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

        public IList<Frameworks> GetApprenticeshipFrameworks()
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
                    item => new Frameworks
                    {
                        Framework = new Framework
                        {
                            CodeName = item.ApprenticeshipFrameworkCodeName,
                            ShortName = item.ApprenticeshipFrameworkShortName,
                            FullName = item.ApprenticeshipFrameworkFullName
                        },
                        Occupation = new Occupation
                        {
                            CodeName = item.ApprenticeshipOccupationCodeName,
                            ShortName = item.ApprenticeshipOccupationShortName,
                            FullName = item.ApprenticeshipOccupationFullName
                        }
                    })
                    .ToList();
            }

            return default(IList<Frameworks>);
        }


        public IList<County> GetCounties()
        {
            var msgId = Guid.NewGuid();
            var request = new GetCountiesRequest
            {
                ExternalSystemId = new Guid(_configManager.GetAppSetting(ReferenceDataUsername)),
                PublicKey = _configManager.GetAppSetting(ReferenceDataPassword),
                MessageId = msgId,
            };

            var rs = default(GetCountiesResponse);
            WcfService<IReferenceData>.Use(client => { rs = client.GetCounties(request); });

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
    }
}
