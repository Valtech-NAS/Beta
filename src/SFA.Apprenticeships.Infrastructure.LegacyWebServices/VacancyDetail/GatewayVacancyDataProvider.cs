namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;

    public class GatewayVacancyDataProvider : IVacancyDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;

        public GatewayVacancyDataProvider(IWcfService<GatewayServiceContract> service)
        {
            _service = service;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var request = new GetVacancyDetailsRequest { VacancyId = vacancyId };

            Logger.Debug("Calling Gateway webservice for vacancy details with ID {0}", vacancyId);

            var response = default(GetVacancyDetailsResponse);
            _service.Use(client => response = client.GetVacancyDetails(request).GetVacancyDetailsResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    Logger.Error("Gateway GetVacancyDetails reported {0} validation error(s): {1}",
                        response.ValidationErrors.Count(), responseAsJson);
                }
                else
                {
                    Logger.Error("Gateway GetVacancyDetails did not respond");
                }

                // TODO: EXCEPTION: use specific exception code
                throw new CustomException("Gateway GetVacancyDetails failed to retrieve vacancy details from legacy system");
            }

            //TODO: return _mapper.Map<Vacancy, VacancyDetail>(response.Vacancy);
            throw new NotImplementedException();
        }
    }
}
