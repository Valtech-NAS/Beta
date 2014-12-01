namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.ServiceModel;
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
            Log("Received", request);

            return Task.Run(() =>
            {
                Log("Submitting", request);

                CreateApplication(request);

                Log("Submitted", request);
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

                    Log("Creating", request);

                    application.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(application);

                    Log("Created", request);

                    SetApplicationStateSubmitted(application, request);
                }
            }
            catch (CustomException ex)
            {
                HandleCustomException(request, ex, application);
            }
            catch (TimeoutException ex)
            {
                LogAndRequeue(request, ex);
            }
            catch (FaultException ex)
            {
                LogAndRequeue(request, ex);
            }
            catch (CommunicationException ex)
            {
                LogAndRequeue(request, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Submit application with Id = {0} request async process failed.", request.ApplicationId), ex);
                throw;
            }
        }

        private void HandleCustomException(SubmitApplicationRequest request, CustomException ex, ApplicationDetail application)
        {
            switch (ex.Code)
            {
                case ErrorCodes.ApplicationDuplicatedError:
                    Logger.Warn("Application has already been submitted to legacy system: Application Id: \"{0}\"",
                        request.ApplicationId);
                    SetApplicationStateSubmitted(application, request);
                    break;

                case ErrorCodes.LegacyCandidateStateError:
                    // TODO: need to consider what else we would do in this event.
                    Logger.Warn("Legacy candidate is in an invalid state: Application Id: \"{0}\"",
                        request.ApplicationId);
                    break;

                default:
                    Logger.Error(
                        string.Format("Submit application with Id = {0} request async process failed.",
                            request.ApplicationId), ex);
                    Requeue(request);
                    break;
            }
        }

        private void LogAndRequeue(SubmitApplicationRequest request, Exception ex)
        {
            Logger.Error(
                string.Format("Submit application with Id = {0} request async process failed.", request.ApplicationId), ex);
            Requeue(request);
        }

        private void SetApplicationStateSubmitted(ApplicationDetail application, SubmitApplicationRequest request)
        {
            Log("Updating", request);

            application.SetStateSubmitted();
            _applicationWriteRepository.Save(application);

            Log("Updated", request);
        }

        private void Requeue(SubmitApplicationRequest request)
        {
            var message = new SubmitApplicationRequest
            {
                ApplicationId = request.ApplicationId
            };
            _messageBus.PublishMessage(message);
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
