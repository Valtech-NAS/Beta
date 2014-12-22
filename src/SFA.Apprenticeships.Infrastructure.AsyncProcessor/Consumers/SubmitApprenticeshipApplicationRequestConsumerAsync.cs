namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SubmitApprenticeshipApplicationRequestConsumerAsync : IConsumeAsync<SubmitApplicationRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public SubmitApprenticeshipApplicationRequestConsumerAsync(
            ILegacyApplicationProvider legacyApplicationProvider,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            IMessageBus messageBus)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "SubmitApprenticeshipApplicationRequestConsumerAsync")]
        public Task Consume(SubmitApplicationRequest request)
        {
            return Task.Run(() =>
            {
                if (request.ProcessTime.HasValue && request.ProcessTime > DateTime.Now)
                {
                    try
                    {
                        _messageBus.PublishMessage(request);
                        return;
                    }
                    catch
                    {
                        Logger.Error("Failed to re-queue deferred 'Submit Apprenticeship Application' request: {{ 'ApplicationId': '{0}' }}", request.ApplicationId);
                        throw;
                    }
                }
                
                Log("Submitting", request);

                CreateApplication(request);
            });
        }

        public void CreateApplication(SubmitApplicationRequest request)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    Logger.Info(
                        "Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
                        applicationDetail.CandidateId);
                    Requeue(request);
                }
                else
                {
                    EnsureApplicationCanBeCreated(applicationDetail);

                    applicationDetail.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);

                    SetApplicationStateSubmitted(applicationDetail);
                }
            }
            catch (CustomException ex)
            {
                HandleCustomException(request, ex, applicationDetail);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Submit apprenticeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
                Requeue(request);
            }
        }

        private void HandleCustomException(SubmitApplicationRequest request, CustomException ex, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            switch (ex.Code)
            {
                case ErrorCodes.ApplicationDuplicatedError:
                    Logger.Warn("Apprenticeship application has already been submitted to legacy system: Application Id: \"{0}\"", request.ApplicationId);
                    SetApplicationStateSubmitted(apprenticeshipApplication);
                    break;

                case ErrorCodes.LegacyCandidateStateError:
                    Logger.Error("Legacy candidate is in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case ErrorCodes.LegacyCandidateNotFoundError:
                    Logger.Error("Legacy candidate was not found. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case ErrorCodes.LegacyVacancyStateError:
                    Logger.Info("Legacy Vacancy was in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    SetStateExpiredOrWithdrawn(apprenticeshipApplication);
                    break;

                case ErrorCodes.ApplicationInIncorrectStateError:
                    Logger.Error(string.Format("Apprenticeship application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), ex);
                    break;

                default:
                    Logger.Warn(string.Format("Submit apprenticeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
                    Requeue(request);
                    break;
            }
        }

        private void SetApplicationStateSubmitted(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            apprenticeshipApplication.SetStateSubmitted();
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
        }

        private void SetStateExpiredOrWithdrawn(ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            apprenticeshipApplication.SetStateExpiredOrWithdrawn();
            _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
        }

        private void Requeue(SubmitApplicationRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }

        private static void EnsureApplicationCanBeCreated(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.AssertState(string.Format("Create apprenticeship application for candidate '{0}'", apprenticeshipApplicationDetail.CandidateId), ApplicationStatuses.Submitting);
        }

        private static void Log(string narrative, SubmitApplicationRequest request)
        {
            Logger.Debug("{0}: Apprenticeship application Id: \"{1}\"", narrative, request.ApplicationId);
        }
    }
}
