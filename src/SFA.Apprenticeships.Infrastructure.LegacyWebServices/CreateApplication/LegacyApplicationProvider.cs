namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateApplication
{
    using System;
    using System.Linq;
    using Application.Candidate;
    using Application.Interfaces.Messaging;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;

    // TODO: AG: NOTIMPL: US352: LegacyApplicationProvider w/s integration.

    /*
     * NOTE: legacy qualification types are:
     * 
     *    BTEC First Diploma / First Certificate
     *    NVQ or SVQ Level 1 / GNVQ Foundation
     *    City & Guilds Craft, RSA Diploma or other trade qualifications/apprenticeship at NVQ level 2
     *    CSEs
     *    GCE O Levels
     *    GCSEs (at A*-C)
     *    NVQ or SVQ Level 2 / GNVQ Intermediate / School Certificate / Matriculation
     *    City & Guilds Advanced Craft, RSA Advanced Diploma or other Advanced trade qualification / apprenticeship at NVQ level 3
     *    GCE A Levels
     *    NVQ or SVQ Level 3 / GNVQ Advanced / NNEB 
     *    BTEC National Diploma / National Certificate / OND / ONC
     *    BTEC HNC/HND
     *    NVQ or SVQ Level 4 / RSA Higher Diploma / Foundation Degree
     *    NVQ or SVQ Level 5
     *    Degree (eg BA, Bsc)/Degree Level Nursing or Teaching Qualification
     *    Postgraduate level (eg PG Dip, MA, MSc, Phd)
     *    Professional Qualifications (eg Chartered Accountant)
     *    Non UK Qualifications
     *    Other
     */

    public class LegacyApplicationProvider : ILegacyApplicationProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyApplicationProvider(IWcfService<GatewayServiceContract> service)
        {
            _service = service;
        }

        public int SubmitApplication(SubmitApplicationRequest request)
        {
            var legacyRequest = new CreateApplicationRequest
            {
            };

            CreateApplicationResponse response = null;

            _service.Use(client => response = client.CreateApplication(legacyRequest));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    // TODO: AG: US352: log actual validation errors (same for other services).
                    Logger.Error(
                        "Legacy CreateApplication reported {0} validation error(s).",
                        response.ValidationErrors.Count());
                }
                else
                {
                    Logger.Error("Legacy CreateApplication did not respond.");
                }

                // TODO: EXCEPTION: should use an application exception type
                throw new Exception("Failed to create candidate in legacy system");
            }

            var legacyApplicationId = response.ApplicationId;

            Logger.Info("Application was successfully created on Legacy web service. LegacyCandidateId={0} ", legacyCandidateId);

            return legacyApplicationId;
        }
    }
}
