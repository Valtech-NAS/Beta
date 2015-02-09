namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using ApplicationsErrorCodes = Application.Interfaces.Applications.ErrorCodes;
    using CandidatesErrorCodes = Application.Interfaces.Candidates.ErrorCodes;
    using CommonErrorCodes = Domain.Entities.ErrorCodes;
    using VacanciesErrorCodes = Application.Interfaces.Vacancies.ErrorCodes;

    public class SubmitApprenticeshipApplicationRequestConsumerAsync : IConsumeAsync<SubmitApprenticeshipApplicationRequest>
    {
        private readonly ILogService _logger;
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
            IMessageBus messageBus, ILogService logger)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "SubmitApprenticeshipApplicationRequestConsumerAsync")]
        public Task Consume(SubmitApprenticeshipApplicationRequest request)
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
                        _logger.Error("Failed to re-queue deferred 'Submit Apprenticeship Application' request: {{ 'ApplicationId': '{0}' }}", request.ApplicationId);
                        throw;
                    }
                }
                
                CreateApplication(request);
            });
        }

        public void CreateApplication(SubmitApprenticeshipApplicationRequest request)
        {
            _logger.Debug("Creating traineeship application Id: {0}", request.ApplicationId);

            var applicationDetail = _apprenticeshipApplicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(applicationDetail.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    _logger.Info("Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
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
                _logger.Error(string.Format("Submit apprenticeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
                Requeue(request);
            }
        }

        private void HandleCustomException(SubmitApprenticeshipApplicationRequest request, CustomException ex, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            switch (ex.Code)
            {
                case ApplicationsErrorCodes.ApplicationDuplicatedError:
                    _logger.Warn("Apprenticeship application has already been submitted to legacy system: Application Id: \"{0}\"", request.ApplicationId);
                    SetApplicationStateSubmitted(apprenticeshipApplication);
                    break;

                case CandidatesErrorCodes.CandidateStateError:
                    _logger.Error("Legacy candidate is in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case CandidatesErrorCodes.CandidateNotFoundError:
                    _logger.Error("Legacy candidate was not found. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case VacanciesErrorCodes.LegacyVacancyStateError:
                    _logger.Info("Legacy Vacancy was in an invalid state. Apprenticeship application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    SetStateExpiredOrWithdrawn(apprenticeshipApplication);
                    break;

                case CommonErrorCodes.EntityStateError:
                    _logger.Error(string.Format("Apprenticeship application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), ex);
                    break;

                default:
                    _logger.Warn(string.Format("Submit apprenticeship application with Id = {0} request async process failed.", request.ApplicationId), ex);
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

        private void Requeue(SubmitApprenticeshipApplicationRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }

        private static void EnsureApplicationCanBeCreated(ApprenticeshipApplicationDetail apprenticeshipApplicationDetail)
        {
            apprenticeshipApplicationDetail.AssertState(string.Format("Create apprenticeship application for candidate '{0}'", apprenticeshipApplicationDetail.CandidateId), ApplicationStatuses.Submitting);
        }
    }
}
