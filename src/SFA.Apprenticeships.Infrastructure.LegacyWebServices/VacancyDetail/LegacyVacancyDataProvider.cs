namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyVacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        private const string UnknownVacancy = "UNKNOWN_VACANCY";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyVacancyDataProvider(
            IWcfService<GatewayServiceContract> service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId)
        {
            var request = new GetVacancyDetailsRequest { VacancyId = vacancyId };

            var response = default(GetVacancyDetailsResponse);

            Logger.Debug("Calling Legacy.GetVacancyDetails webservice for vacancy details with ID {0}", vacancyId);

            _service.Use("SecureService", client => response = client.GetVacancyDetails(request).GetVacancyDetailsResponse);

            if (IsThereAnyErrorOn(response))
            {
                if (IsThereAnyValidationErrorOn(response))
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);
                    Logger.Info("Legacy.GetVacancyDetails reported {0} validation error(s): {1}", response.ValidationErrors.Count(), responseAsJson);
                    if (VacancyDoesntExist(response))
                    {
                        Logger.Info("Unknown vacancy ({0}). Returning null.", vacancyId);
                        return null;
                    }
                }
                else
                {
                    Logger.Info("Legacy.GetVacancyDetails did not respond");
                }

                var message = string.Format("Legacy.GetVacancyDetails failed to retrieve vacancy details from legacy system for vacancyId {0}", vacancyId);
                throw new CustomException(message, ErrorCodes.GatewayServiceFailed);
            }

            var vacancyDetail = GetVacancyDetailFrom(response);

            if (!VacancyHasExpired(vacancyDetail)) return vacancyDetail;

            Logger.Info("Vacancy ({0}) closing date has expired. Returning null.", vacancyId);
            return null;
        }

        private static bool VacancyDoesntExist(GetVacancyDetailsResponse response)
        {
            return response.ValidationErrors.Any(e => e.ErrorCode == UnknownVacancy);
        }

        private static bool VacancyHasExpired(TVacancyDetail vacancyDetail)
        {
            return vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime();
        }

        private TVacancyDetail GetVacancyDetailFrom(GetVacancyDetailsResponse response)
        {
            var vacancyDetail = _mapper.Map<Vacancy, TVacancyDetail>(response.Vacancy);
            return vacancyDetail;
        }

        private static bool IsThereAnyValidationErrorOn(GetVacancyDetailsResponse response)
        {
            return response != null && response.ValidationErrors != null && response.ValidationErrors.Any();
        }

        private static bool IsThereAnyErrorOn(GetVacancyDetailsResponse response)
        {
            return response == null || response.Vacancy == null || (response.ValidationErrors != null && response.ValidationErrors.Any());
        }
    }
}
