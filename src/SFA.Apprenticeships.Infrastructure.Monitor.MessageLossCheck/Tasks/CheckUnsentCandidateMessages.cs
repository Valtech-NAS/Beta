namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;
    using Monitor.Tasks;
    using Repository;

    public class CheckUnsentCandidateMessages : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly ICandidateDiagnosticsRepository _candidateDiagnosticsRepository;
        private readonly IMessageBus _messageBus;

        public CheckUnsentCandidateMessages(ICandidateDiagnosticsRepository candidateDiagnosticsRepository, IMessageBus messageBus, ILogService logger)
        {
            _candidateDiagnosticsRepository = candidateDiagnosticsRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Unsent Candidate Messages"; }
        }

        public void Run()
        {
            var sb = new StringBuilder("The following actions were taken to resolve issues with candidates:");
            sb.AppendLine();

            var candidatesToRequeue = _candidateDiagnosticsRepository.GetActivatedCandidatesWithUnsetLegacyId().ToList();
            
            foreach (var candidate in candidatesToRequeue)
            {
                _logger.Info("Re-queuing create candidate message for candidate id: {0}", candidate.EntityId);

                var message = new CreateCandidateRequest
                {
                    CandidateId = candidate.EntityId
                };

                _messageBus.PublishMessage(message);

                var requeuedMessage = string.Format("Re-queued create candidate message for candidate id: {0}", candidate.EntityId);
                _logger.Info(requeuedMessage);
                sb.AppendLine(requeuedMessage);
            }

            if (!candidatesToRequeue.Any())
            {
                ActionsTaken = false;
                return;
            }

            ActionsTaken = true;

            //Wait 5 seconds to allow messages to be processed. Nondeterministic of course
            Thread.Sleep(TimeSpan.FromSeconds(5));
            
            var activatedCandidatesWithUnsetLegacyId = _candidateDiagnosticsRepository.GetActivatedCandidatesWithUnsetLegacyId().ToList();
            if (activatedCandidatesWithUnsetLegacyId.Any())
            {
                sb.AppendLine("The actions taken did not resolve the following issues with candidates:");
                activatedCandidatesWithUnsetLegacyId.ForEach(c => sb.AppendLine(string.Format("Candidate with id: {0} is activated but has an unset legacy candidate id", c.EntityId)));
                _logger.Error(sb.ToString());
                ActionsSuccessful = false;
            }
            else
            {
                sb.AppendLine("The actions taken appear to have resolved the issues");
                _logger.Warn(sb.ToString());
                ActionsSuccessful = true;
            }
        }

        public bool ActionsTaken { get; private set; }

        public bool ActionsSuccessful { get; private set; }
    }
}