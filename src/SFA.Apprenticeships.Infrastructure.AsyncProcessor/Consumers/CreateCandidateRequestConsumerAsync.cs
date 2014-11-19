namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System.Threading.Tasks;
    using Application.Interfaces.Messaging;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using NLog;

    public class CreateCandidateRequestConsumerAsync : IConsumeAsync<CreateCandidateRequest>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;

        public CreateCandidateRequestConsumerAsync(
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IMessageBus messageBus)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _messageBus = messageBus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "CreateCandidateRequestConsumerAsync")]
        public Task Consume(CreateCandidateRequest request)
        {
            Log("Received", request);

            return Task.Run(() =>
            {
                Log("Submitting", request);

                CreateCandidate(request);

                Log("Submitted", request);
            });
        }

        private void CreateCandidate(CreateCandidateRequest request)
        {
            var candidate = _candidateReadRepository.Get(request.CandidateId, true);
            var user = _userReadRepository.Get(request.CandidateId);

            //user.AssertState();
            //EnsureApplicationCanBeCreated(candidate);

            //try
            //{
                Log("Creating", request);

                //candidate.LegacyApplicationId = _legacyApplicationProvider.CreateApplication(candidate);

                Log("Created", request);

                Log("Updating", request);

                //candidate.SetStateSubmitted();
                //_applicationWriteRepository.Save(candidate);

                Log("Updated", request);
            //}
            //catch (CustomException ex)
            //{
                //if (ex.Code != ErrorCodes.ApplicationDuplicatedError)
                //{
                //    // re-queue application for submission
                //    var message = new CreateCandidateRequest
                //    {
                //        ApplicationId = request.ApplicationId
                //    };
                //    _messageBus.PublishMessage(message); 
                //}
            //}
        }

        private static void Log(string narrative, CreateCandidateRequest request)
        {
            Logger.Debug("{0}: Candidate Id: \"{1}\"", narrative, request.CandidateId);
        }
    }
}
