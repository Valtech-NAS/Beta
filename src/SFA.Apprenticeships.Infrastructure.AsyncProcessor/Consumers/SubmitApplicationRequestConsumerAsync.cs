namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate.Strategies;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class SubmitApplicationRequestConsumerAsync : IConsumeAsync<SubmitApplicationRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ILegacyApplicationProvider _legacyApplicationProvider;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IMessageBus _messageBus;

        public SubmitApplicationRequestConsumerAsync(
            ILegacyApplicationProvider legacyApplicationProvider,
            IApplicationReadRepository applicationReadRepository,
            IApplicationWriteRepository applicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            IMessageBus messageBus)
        {
            _legacyApplicationProvider = legacyApplicationProvider;
            _applicationReadRepository = applicationReadRepository;
            _applicationWriteRepository = applicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "SubmitApplicationRequestConsumerAsync")]
        public Task Consume(SubmitApplicationRequest request)
        {
            return Task.Run(() =>
            {
                if (request.ProcessTime.HasValue && request.ProcessTime > DateTime.Now)
                {
                    _messageBus.PublishMessage(request);
                    return;
                }
                
                Log("Submitting", request);

                CreateApplication(request);
            });
        }

        public void CreateApplication(SubmitApplicationRequest request)
        {
            var application = _applicationReadRepository.Get(request.ApplicationId, true);

            try
            {
                var candidate = _candidateReadRepository.Get(application.CandidateId, true);

                if (candidate.LegacyCandidateId == 0)
                {
                    Logger.Info(
                        "Candidate with Id: {0} has not been created in the legacy system. Message will be requeued",
                        application.CandidateId);
                    Requeue(request);
                }
                else
                {
                    EnsureApplicationCanBeCreated(application);

                    application.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(application);

                    SetApplicationStateSubmitted(application);
                }
            }
            catch (CustomException ex)
            {
                HandleCustomException(request, ex, application);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Submit application with Id = {0} request async process failed.", request.ApplicationId), ex);
                Requeue(request);
            }
        }

        private void HandleCustomException(SubmitApplicationRequest request, CustomException ex, ApplicationDetail application)
        {
            switch (ex.Code)
            {
                case ErrorCodes.ApplicationDuplicatedError:
                    Logger.Warn("Application has already been submitted to legacy system: Application Id: \"{0}\"", request.ApplicationId);
                    SetApplicationStateSubmitted(application);
                    break;

                case ErrorCodes.LegacyCandidateStateError:
                    // TODO: need to consider what else we would do in this event.
                    Logger.Error("Legacy candidate is in an invalid state. Application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case ErrorCodes.LegacyCandidateNotFoundError:
                    // TODO: need to consider what else we would do in this event.
                    Logger.Error("Legacy candidate was not found. Application cannot be processed: Application Id: \"{0}\"", request.ApplicationId);
                    break;

                case ErrorCodes.ApplicationInIncorrectStateError:
                    Logger.Error(string.Format("Application is in an invalid state: Application Id: \"{0}\"", request.ApplicationId), ex);
                    break;

                default:
                    Logger.Error(string.Format("Submit application with Id = {0} request async process failed.", request.ApplicationId), ex);
                    Requeue(request);
                    break;
            }
        }

        private void SetApplicationStateSubmitted(ApplicationDetail application)
        {
            application.SetStateSubmitted();
            _applicationWriteRepository.Save(application);
        }

        private void Requeue(SubmitApplicationRequest request)
        {
            request.ProcessTime = request.ProcessTime.HasValue ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(30);
            _messageBus.PublishMessage(request);
        }

        private static void EnsureApplicationCanBeCreated(ApplicationDetail applicationDetail)
        {
            var message = string.Format("Cannot create application with Id \"{0}\" for candidate with Id \"{1}\" in state: \"{2}\".",
                applicationDetail.EntityId, applicationDetail.CandidateId, applicationDetail.Status);

            applicationDetail.AssertState(message, ApplicationStatuses.Submitting);
        }

        private static void Log(string narrative, SubmitApplicationRequest request)
        {
            Logger.Debug("{0}: Application Id: \"{1}\"", narrative, request.ApplicationId);
        }
    }
}
