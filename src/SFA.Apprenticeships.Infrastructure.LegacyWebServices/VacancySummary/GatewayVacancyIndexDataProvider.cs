namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using Application.VacancyEtl;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;
    using VacancySummary = Domain.Entities.Vacancies.VacancySummary;

    public class GatewayVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;

        public GatewayVacancyIndexDataProvider(IWcfService<GatewayServiceContract> service)
        {
            _service = service;
        }

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            var request = new GetVacancySummaryRequest { PageNumber = 1 };

            Logger.Debug("Calling Gateway webservice for vacancy index page count");

            var response = default(GetVacancySummaryResponse);
            _service.Use(client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                // TODO: EXCEPTION: use specific exception code
                throw new CustomException("Gateway GetVacancySummaries failed to retrieve page count from legacy system");
            }

            return response.TotalPages;
        }

        public IEnumerable<VacancySummary> GetVacancySummaries(VacancyLocationType vacancyLocationType, int page)
        {
            //todo: remove vacancyLocationType arg as results will contain all types

            var request = new GetVacancySummaryRequest { PageNumber = page };

            Logger.Debug("Calling Gateway webservice for vacancy data page {0}", page);

            var response = default(GetVacancySummaryResponse);
            _service.Use(client => response = client.GetVacancySummaries(request));

            if (response == null)
            {
                Logger.Error("Gateway GetVacancySummaries did not respond");

                // TODO: EXCEPTION: use specific exception code
                throw new CustomException("Gateway GetVacancySummaries failed to retrieve page '" + page + "' from legacy system");
            }

            //TODO: return _mapper.Map<GatewayServiceProxy.VacancySummary[], IEnumerable<VacancySummary>>(response.VacancySummaries);
            throw new NotImplementedException();
        }
    }
}
