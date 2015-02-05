﻿namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
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

    public class CheckUnsentTraineeshipApplicationMessages : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly ITraineeshipApplicationDiagnosticsRepository _applicationDiagnosticsRepository;
        private readonly IMessageBus _messageBus;

        public CheckUnsentTraineeshipApplicationMessages(ITraineeshipApplicationDiagnosticsRepository applicationDiagnosticsRepository, IMessageBus messageBus, ILogService logger)
        {
            _applicationDiagnosticsRepository = applicationDiagnosticsRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check Unsent Traineeship Application Messages"; }
        }

        public void Run()
        {
            var sb = new StringBuilder("The following actions were taken to resolve issues with traineeship applications:");
            sb.AppendLine();

            var applicationsToRequeue = _applicationDiagnosticsRepository.GetApplicationsForValidCandidatesWithUnsetLegacyId().ToList();

            foreach (var application in applicationsToRequeue)
            {
                _logger.Info("Requeuing create traineeship application message for application id: {0}", application.EntityId);

                var message = new SubmitTraineeshipApplicationRequest
                {
                    ApplicationId = application.EntityId
                };

                _messageBus.PublishMessage(message);

                var requeuedMessage = string.Format("Requeued create traineeship application message for candidate id: {0}", application.EntityId);
                _logger.Info(requeuedMessage);
                sb.AppendLine(requeuedMessage);
            }

            if (!applicationsToRequeue.Any()) return;

            //Wait 5 seconds to allow messages to be processed. Nondeterministic of course
            Thread.Sleep(TimeSpan.FromSeconds(5));

            var applicationsForValidCandidatesWithUnsetLegacyId = _applicationDiagnosticsRepository.GetApplicationsForValidCandidatesWithUnsetLegacyId().ToList();
            if (applicationsForValidCandidatesWithUnsetLegacyId.Any())
            {
                sb.AppendLine("The actions taken did not resolve the following issues with traineeship applications:");
                applicationsForValidCandidatesWithUnsetLegacyId.ForEach(a => sb.AppendLine(string.Format("Application with id: {0} is associated with a valid candidate but has an unset legacy application id", a.EntityId)));
                _logger.Error(sb.ToString());
            }
            else
            {
                sb.AppendLine("The actions taken appear to have resolved the issues");
                _logger.Warn(sb.ToString());
            }
        }
    }
}