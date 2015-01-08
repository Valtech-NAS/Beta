namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Application.Candidate;
    using Application.Interfaces.Messaging;
    using Domain.Interfaces.Messaging;
    using Monitor.Tasks;
    using NLog;
    using Repository;

    public class CheckUnsentCandidateMessages : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICandidateDiagnosticsRepository _candidateDiagnosticsRepository;
        private readonly IMessageBus _messageBus;

        public CheckUnsentCandidateMessages(ICandidateDiagnosticsRepository candidateDiagnosticsRepository, IMessageBus messageBus)
        {
            _candidateDiagnosticsRepository = candidateDiagnosticsRepository;
            _messageBus = messageBus;
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
                Logger.Info("Requeuing create candidate message for candidate id: {0}", candidate.EntityId);

                var message = new CreateCandidateRequest
                {
                    CandidateId = candidate.EntityId
                };

                _messageBus.PublishMessage(message);

                var requeuedMessage = string.Format("Requeued create candidate message for candidate id: {0}", candidate.EntityId);
                Logger.Info(requeuedMessage);
                sb.AppendLine(requeuedMessage);
            }

            if (!candidatesToRequeue.Any()) return;

            //Wait 5 seconds to allow messages to be processed. Nondeterministic of course
            Thread.Sleep(TimeSpan.FromSeconds(5));
            
            var activatedCandidatesWithUnsetLegacyId = _candidateDiagnosticsRepository.GetActivatedCandidatesWithUnsetLegacyId().ToList();
            if (activatedCandidatesWithUnsetLegacyId.Any())
            {
                sb.AppendLine("The actions taken did not resolve the following issues with candidates:");
                activatedCandidatesWithUnsetLegacyId.ForEach(c => sb.AppendLine(string.Format("Candidate with id: {0} is activated but has an unset legacy candidate id", c.EntityId)));
                Logger.Error(sb.ToString());
            }
            else
            {
                sb.AppendLine("The actions taken appear to have resolved the issues");
                Logger.Warn(sb.ToString());
            }
        }
    }
}